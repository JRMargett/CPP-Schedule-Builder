# CPP Schedule Builder

CPP Schedule Builder is a Windows desktop application for building and optimizing class schedules. The program imports course data, processes available lecture sections, and generates optimized schedules based on the selected criteria.

## Requirements

Before running the project, make sure you have the following installed:

- Windows
- .NET 8
- Python
- `requests`
- `RateMyProfessorAPI`

## Python Setup

This project uses a Python helper script to retrieve professor ratings from Rate My Professors.

Source: https://pypi.org/project/RateMyProfessorAPI/

Install the required Python packages with:

```bash
python -m pip install -r requirements.txt
```

The `requirements.txt` file should include:

```txt
RateMyProfessorAPI==1.3.6
requests
```

## Testing the Program

To test the program, download the following file from the GitHub repository:

```text
TestingCourses.txt
```

After downloading the file:

1. Run the program.
2. Click **Import**.
3. Select `TestingCourses.txt`.
4. Wait for the course data to load.
5. Select one of the optimization options using the corresponding radio button.
6. Click **Run**.
7. Allow the program time to process the course information.
8. View the generated output schedule.

## Optimization Options

The program can generate schedules based on different optimization goals, such as:

- Minimizing commute issues
- Prioritizing professor ratings
- Avoiding class time conflicts
- Building a valid finalized schedule from imported course data

## Notes

The Rate My Professors integration depends on a Python package rather than an official public API. Because of that, the professor rating feature may stop working if the package or Rate My Professors changes.

## AI Usage Disclosure

AI was used in this project to help implement and improve several features, including:

- Integrating the Python-based Rate My Professor API helper
- Gathering and organizing course information
- Setting up the print/print-preview functionality
- Improving the codebase structure and classes
- Writing and improving this README file
