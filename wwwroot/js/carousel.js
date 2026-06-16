let nextDom = document.getElementById('next');
let prevDom = document.getElementById('prev');
let carouselDom = document.querySelector('.carousel');
let listDom = document.querySelector('.carousel .list');
let items = document.querySelectorAll('.carousel .list .item');
let thumbnails = document.querySelectorAll('.carousel .thumbnail .item');

let currentIndex = 0;

function updateCarousel() {
    // Обрати внимание на бэктики `...`
    listDom.style.transform = `translateX(-${currentIndex * 100}%)`;
    thumbnails.forEach((thumbnail, index) => {
        thumbnail.classList.toggle('active', index === currentIndex);
    });
}

nextDom.addEventListener('click', () => {
    currentIndex = (currentIndex + 1) % items.length;
    updateCarousel();
});

prevDom.addEventListener('click', () => {
    currentIndex = (currentIndex - 1 + items.length) % items.length;
    updateCarousel();
});

// Автопрокрутка
let autoScroll = setInterval(() => {
    nextDom.click();
}, 5000);

// Останавливаем автопрокрутку при наведении мыши
carouselDom.addEventListener('mouseenter', () => clearInterval(autoScroll));

// Возобновляем автопрокрутку, когда убираем мышь
carouselDom.addEventListener('mouseleave', () => {
    autoScroll = setInterval(() => {
        nextDom.click();
    }, 5000);
});

// Инициализация
updateCarousel();
