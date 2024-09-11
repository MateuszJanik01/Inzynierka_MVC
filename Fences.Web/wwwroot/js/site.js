// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const footerYear = document.querySelector('.footer-year');

const handleCurrentYear = () => {
	const year = (new Date).getFullYear();
	footerYear.innerText = year;
}

handleCurrentYear();

//Form contact Validation

// Funkcja resetująca style (przywraca normalne tło)
function resetStyles() {
    document.getElementById('name').style.backgroundColor = "";
    document.getElementById('email').style.backgroundColor = "";
    document.getElementById('content').style.backgroundColor = "";
}

// Funkcja walidująca
function validateForm() {
    resetStyles(); // Resetowanie styli przed każdą walidacją
    let name = document.getElementById('name').value.trim();
    let email = document.getElementById('email').value.trim();
    let content = document.getElementById('content').value.trim();
    let emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    let isValid = true;

    // Walidacja pola 'Imię i Nazwisko'
    if (name === "" || name.length < 3) {
        document.getElementById('name').style.backgroundColor = "#f0a7a7";
        isValid = false;
    }

    // Walidacja pola 'E-mail'
    if (email === "" || !emailPattern.test(email)) {
        document.getElementById('email').style.backgroundColor = "#f0a7a7";
        isValid = false;
    }

    // Walidacja pola 'Treść wiadomości'
    if (content === "" || content.length < 10) {
        document.getElementById('content').style.backgroundColor = "#f0a7a7";
        isValid = false;
    }

    return isValid;
}

// Funkcja czyszcząca formularz
function clearForm() {
    document.getElementById('name').value = "";
    document.getElementById('email').value = "";
    document.getElementById('content').value = "";
}

// Przypisanie funkcji walidującej do przycisku
document.getElementById('submitFormBtn').addEventListener('click', function () {
    if (validateForm()) {
        alert("Wiadomość została wysłana!");
        clearForm();
    }
});

// Funkcje do galerii

// Pobranie elementów
const galleryItems = document.querySelectorAll('.gallery-item');

const fullScreenGallery = document.getElementById('fullScreenGallery');
const fullScreenImage = document.getElementById('fullScreenImage');

const closeFullScreen = document.getElementById('closeFullScreen');
const prevImage = document.getElementById('prevImage');
const nextImage = document.getElementById('nextImage');

let currentIndex = 0;

// Funkcja otwierająca pełnoekranowy widok
galleryItems.forEach((item, index) => {
    item.addEventListener('click', function () {
        currentIndex = index;
        let src = this.getAttribute('src');
        src = src.slice(0, -6);
        src += "XL.jpg";
        fullScreenImage.setAttribute('src', src);
        fullScreenGallery.style.display = 'flex';
    });
});

// Zamknięcie pełnoekranowego widoku po kliknięciu w tło
fullScreenGallery.addEventListener('click', function (event) {
    if (event.target === fullScreenGallery) {
        fullScreenGallery.style.display = 'none';
    }
});

// Zamknięcie pełnoekranowego widoku po kliknięciu na przycisk zamknięcia
closeFullScreen.addEventListener('click', function () {
    fullScreenGallery.style.display = 'none';
});

// Funkcja do przechodzenia do następnego zdjęcia
nextImage.addEventListener('click', function () {
    currentIndex = (currentIndex + 1) % galleryItems.length;
    updateFullScreenImage();
});

// Funkcja do przechodzenia do poprzedniego zdjęcia
prevImage.addEventListener('click', function () {
    currentIndex = (currentIndex - 1 + galleryItems.length) % galleryItems.length;
    updateFullScreenImage();
});

// Funkcja aktualizująca obraz w pełnym ekranie
function updateFullScreenImage() {
    let src = galleryItems[currentIndex].getAttribute('src');
    src = src.slice(0, -6);
    src += "XL.jpg";
    fullScreenImage.setAttribute('src', src);
}
