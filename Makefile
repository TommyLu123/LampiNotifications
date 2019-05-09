.PHONY: install

FOLDER :=
ifeq ($(OS),Windows_NT)
	FOLDER:=Scripts
else
	FOLDER:=bin
endif

VENV_NAME?=venv
VENV_ACTIVATE=. $(VENV_NAME)/$(FOLDER)/activate
PYTHON=${VENV_NAME}/$(FOLDER)/python

install: create activate

create:
ifeq ($(OS),Windows_NT)
	virtualenv venv
else
	python3 -m venv venv
endif

activate:
	source $(VENV_NAME)$(VENV_PATH)/$(FOLDER)/activate; \
	$(VENV_NAME)$(VENV_PATH)/$(FOLDER)/pip install -r requirements.txt; \

clean:
	rm -rf venv