
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
        hamburgerIcon.style.display = "none";
        crossIcon.style.display = "none";
        menuContainer.classList.remove("nav-link-container-mobile");
    } else {
        hamburgerIcon.style.display = "block";
    }
}

mediaQuery.addEventListener("change", checkScreenSize);

checkScreenSize();

window.addEventListener('resize', checkScreenSize);

document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname;
    const highlightedLinkClass = 'highlighted-link';
    document.querySelectorAll('nav a, nav svg').forEach(link => link.classList.remove(highlightedLinkClass));

    if (currentPath.includes("/Home")) {
        document.getElementById('home-link').classList.add(highlightedLinkClass);
    } else if (currentPath.includes("/Shop")) {
        document.getElementById('shop-link').classList.add(highlightedLinkClass);
    } else if (currentPath.includes("/Wishlist")) {
        document.getElementById('wishlist-link').classList.add(highlightedLinkClass);
    } else if (currentPath.includes("/Cart")) {
        document.getElementById('cart-link').classList.add(highlightedLinkClass);
    } else if (["/Profile", "/Purchases", "/Address"].some(path => currentPath.includes(path))) {
        document.getElementById('profile-link').classList.add(highlightedLinkClass);
    }
});