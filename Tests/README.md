# Netflix Automation Tests

## Setup
- Install Python 3.7 or higher
- Create a virtual environment
```bash
python3 -m venv .venv
```
- Activate the virtual environment
```bash
source .venv/bin/activate
```
- Install the required packages
```bash
pip install -r requirements.txt
```

## Running the tests
- Run the tests
```bash
WEBSITE_URL=http://localhost:5261 SLEEP_TIME=3 pytest -x tests/
```
