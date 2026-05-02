import json
import re
import sys
from urllib.parse import quote_plus

import requests


CPP_SCHOOL_ID = 13914
CPP_SCHOOL_REF = "U2Nob29sLTEzOTE0"
CPP_SCHOOL_NAME = "Cal Poly Pomona"

HEADERS = {
    "User-Agent": (
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
        "AppleWebKit/537.36 (KHTML, like Gecko) "
        "Chrome/124.0 Safari/537.36"
    ),
    "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
}

TEACHER_PATTERN = re.compile(
    r'"__typename":"Teacher",'
    r'"id":"(?P<id>[^"]+)",'
    r'"legacyId":(?P<legacy_id>\d+),'
    r'"avgRating":(?P<rating>-?\d+(?:\.\d+)?),'
    r'"numRatings":(?P<num_ratings>\d+),'
    r'"wouldTakeAgainPercent":(?P<would_take_again>-?\d+(?:\.\d+)?),'
    r'"avgDifficulty":(?P<difficulty>-?\d+(?:\.\d+)?),'
    r'"department":(?P<department>null|"(?:\\.|[^"\\])*"),'
    r'"school":\{"__ref":"' + CPP_SCHOOL_REF + r'"\},'
    r'"firstName":"(?P<first_name>(?:\\.|[^"\\])*)",'
    r'"lastName":"(?P<last_name>(?:\\.|[^"\\])*)"'
)


def main():
    if len(sys.argv) < 3:
        print_json({
            "found": False,
            "error": "Missing school name or professor name."
        })
        return

    professor_name = sys.argv[2]

    try:
        professor = get_cpp_professor(professor_name)
        if professor is None:
            print_json({
                "found": False,
                "school": CPP_SCHOOL_NAME,
                "error": "Professor not found at Cal Poly Pomona."
            })
            return

        print_json({
            "found": True,
            "school": CPP_SCHOOL_NAME,
            "name": professor["name"],
            "department": professor["department"],
            "rating": professor["rating"],
            "difficulty": professor["difficulty"],
            "numRatings": professor["numRatings"],
            "wouldTakeAgain": professor["wouldTakeAgain"]
        })
    except Exception as ex:
        print_json({
            "found": False,
            "school": CPP_SCHOOL_NAME,
            "error": str(ex)
        })


def get_cpp_professor(professor_name):
    normalized_name = normalize_name(professor_name)
    if not normalized_name:
        return None

    search_url = (
        f"https://www.ratemyprofessors.com/search/professors/"
        f"{CPP_SCHOOL_ID}?q={quote_plus(professor_name)}"
    )
    response = requests.get(search_url, headers=HEADERS, timeout=15)
    response.raise_for_status()

    professors = parse_professors(response.text)
    if not professors:
        return None

    exact_matches = [
        professor for professor in professors
        if normalize_name(professor["name"]) == normalized_name
    ]
    if exact_matches:
        return best_professor(exact_matches)

    name_parts = set(normalized_name.split())
    partial_matches = [
        professor for professor in professors
        if name_parts and name_parts.issubset(set(normalize_name(professor["name"]).split()))
    ]
    if partial_matches:
        return best_professor(partial_matches)

    return max(professors, key=lambda professor: professor["numRatings"])


def parse_professors(page_text):
    professors = []

    for match in TEACHER_PATTERN.finditer(page_text):
        first_name = decode_json_string(match.group("first_name"))
        last_name = decode_json_string(match.group("last_name"))
        department = decode_json_value(match.group("department"))
        rating = float(match.group("rating"))
        difficulty = float(match.group("difficulty"))
        would_take_again = float(match.group("would_take_again"))

        professors.append({
            "name": f"{first_name} {last_name}".strip(),
            "department": department,
            "rating": rating,
            "difficulty": difficulty if difficulty > 0 else None,
            "numRatings": int(match.group("num_ratings")),
            "wouldTakeAgain": would_take_again if would_take_again >= 0 else None
        })

    return professors


def best_professor(professors):
    return max(
        professors,
        key=lambda professor: (
            professor["numRatings"],
            professor["rating"],
            -(professor["difficulty"] or 5)
        )
    )


def normalize_name(name):
    return " ".join(re.sub(r"[^a-zA-Z\s]", " ", name).lower().split())


def decode_json_string(value):
    return json.loads(f'"{value}"')


def decode_json_value(value):
    return None if value == "null" else json.loads(value)


def print_json(value):
    print(json.dumps(value))


if __name__ == "__main__":
    main()
