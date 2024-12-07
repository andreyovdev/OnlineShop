const hamburgerIcon = document.querySelector(".hamburger-icon");
const crossIcon = document.querySelector(".cross-tab-icon-mobile");
const menuContainer = document.querySelector(".nav-link-container");
const mediaQuery = window.matchMedia("(max-width: 768px)");

function openMenu() {
    menuContainer.classList.add("nav-link-container-mobile");
    hamburgerIcon.style.display = "none";
    crossIcon.style.display = "block";
}

function closeMenu() {
    menuContainer.classList.remove("nav-link-container-mobile");
    hamburgerIcon.style.display = "block";
    crossIcon.style.display = "none";
}

hamburgerIcon.addEventListener("click", openMenu);

crossIcon.addEventListener("click", closeMenu);

function checkScreenSize() {
    if (!mediaQuery.matches) {
        menuContainer.classList.remove("nav-link-container-mobile");
        hamburgerIcon.style.display = "block";
        crossIcon.style.display = "none";
    }
}

mediaQuery.addEventListener("change", checkScreenSize);

checkScreenSize();

function toggleIconsBasedOnScreenWidth() {
    if (window.innerWidth <= 768) {
        hamburgerIcon.style.display = 'block';
        crossIcon.style.display = 'none';
    } else {
        hamburgerIcon.style.display = 'none';
        crossIcon.style.display = 'none';
    }
}

window.addEventListener('resize', toggleIconsBasedOnScreenWidth);

toggleIconsBasedOnScreenWidth();

hamburgerIcon.addEventListener('click', () => {
    hamburgerIcon.style.display = 'none';
    crossIcon.style.display = 'block';
});

crossIcon.addEventListener('click', () => {
    crossIcon.style.display = 'none';
    hamburgerIcon.style.display = 'block';
});
