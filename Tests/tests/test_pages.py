import os
import time

import pytest
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import Select

SLEEP_TIME = os.getenv("SLEEP_TIME", 1)
WEBSITE_URL = os.getenv("WEBSITE_URL", "http://localhost:5261")


class BaseTestPage:
    URL = WEBSITE_URL

    @pytest.fixture(autouse=True)
    def browser_setup_and_teardown(self):
        self.browser = webdriver.Chrome()
        self.browser.maximize_window()
        self.browser.implicitly_wait(10)
        self.browser.get(self.URL)

        yield

        if SLEEP_TIME and SLEEP_TIME.isdigit():
            time.sleep(int(SLEEP_TIME))
        self.browser.close()
        self.browser.quit()


class TestFormPage(BaseTestPage):
    URL = f"{WEBSITE_URL}/Home/Index"

    def test_title(self):
        assert self.browser.title == "Formulario de Películas - Estilo Netflix"

    def test_header_text(self):
        header = self.browser.find_element(By.CSS_SELECTOR, "div.form-header h2")
        assert header.text == "Formulario de Películas"

    def test_header_logo(self):
        logo = self.browser.find_element(By.CSS_SELECTOR, "div.form-header img")
        assert logo.get_attribute("src") == "https://upload.wikimedia.org/wikipedia/commons/7/75/Netflix_icon.svg"
        assert logo.get_attribute("alt") == "Netflix Logo"

    def test_form_elements(self):
        form_elements = self.browser.find_elements(By.CSS_SELECTOR, "form div.form-group")
        assert len(form_elements) == 6

    def test_form_elements_labels(self):
        form_elements = self.browser.find_elements(By.CSS_SELECTOR, "form div.form-group label[for]")
        labels = (
            "Tu película favorita:",
            "Géneros favoritos:",
            "Plataformas de streaming que usas:",
            "Actor/actriz favorito:",
            "Describe tu escena favorita:",
            "Fecha y hora de la última película vista:",
        )
        for element in form_elements:
            assert element.text in labels

    def test_form_elements_input(self):
        form_elements = self.browser.find_elements(By.CSS_SELECTOR, "form div.form-group input:not([type='submit'])")
        assert len(form_elements) == 7

    def test_form_submit_data(self):
        self.browser.find_element(By.NAME, "pelicula").send_keys("nombre")
        self.browser.find_element(By.NAME, "genero").send_keys("accion")
        self.browser.find_element(By.CSS_SELECTOR, "form input[name='plataformas'][value='netflix']").click()

        actor_select = Select(self.browser.find_element(By.NAME, "actor"))
        actor_select.select_by_visible_text("Denzel Washington")
        self.browser.find_element(By.NAME, "comentario").send_keys("Testing")

        datetime_input = self.browser.find_element(By.NAME, "datetime")
        datetime_input.send_keys("12/07/2024")
        datetime_input.send_keys(Keys.TAB)
        datetime_input.send_keys("13:00")

        submit_button = self.browser.find_element(By.CSS_SELECTOR, "form button[type='submit']")
        submit_button.click()

        assert self.browser.current_url == f"{WEBSITE_URL}/Home/Inicio"
        assert self.browser.title == "Listado de Películas Guardadas"

        last_row = self.browser.find_element(By.CSS_SELECTOR, "div.container table tbody tr:last-child")
        assert last_row.find_element(By.CSS_SELECTOR, "td:nth-child(1)").text == "nombre"
        assert last_row.find_element(By.CSS_SELECTOR, "td:nth-child(2)").text == "accion"
        assert last_row.find_element(By.CSS_SELECTOR, "td:nth-child(3)").text == "netflix"
        assert last_row.find_element(By.CSS_SELECTOR, "td:nth-child(4)").text == "washington"
        assert last_row.find_element(By.CSS_SELECTOR, "td:nth-child(5)").text == "2024-07-12 13:00"
        assert (
            last_row.find_element(By.CSS_SELECTOR, "td:nth-child(6)").find_element(By.CSS_SELECTOR, "button").text
            == "Eliminar"
        )


class TestListPage(BaseTestPage):
    URL = f"{WEBSITE_URL}/Home/Inicio"

    def test_title(self):
        assert self.browser.title == "Listado de Películas Guardadas"

    def test_header_text(self):
        header = self.browser.find_element(By.CSS_SELECTOR, "div.container div.header h2")
        assert header.text == "Listado de Películas Guardadas"

    def test_return_button(self):
        return_button = self.browser.find_element(By.CSS_SELECTOR, "div.container div.button-container a")
        assert return_button.text == "Volver al Formulario"
        assert return_button.get_attribute("href") == f"{WEBSITE_URL}/Home/Index"

    def test_delete_button(self):
        delete_button = self.browser.find_element(By.CSS_SELECTOR, "div.container table tbody tr:last-child button")
        assert delete_button.text == "Eliminar"

    def test_delete_form(self):
        initial_rows = len(self.browser.find_elements(By.CSS_SELECTOR, "div.container table tbody tr"))
        delete_button = self.browser.find_element(By.CSS_SELECTOR, "div.container table tbody tr:last-child button")
        delete_button.click()

        assert self.browser.current_url == f"{WEBSITE_URL}/Home/Inicio"
        assert self.browser.title == "Listado de Películas Guardadas"
        assert len(self.browser.find_elements(By.CSS_SELECTOR, "div.container table tbody tr")) == initial_rows - 1
