// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const footerYear = document.querySelector('.footer-year');

const handleCurrentYear = () => {
	const year = (new Date).getFullYear();
	footerYear.innerText = year;
}

handleCurrentYear();

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
