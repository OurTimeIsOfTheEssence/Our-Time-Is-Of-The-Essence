// ——————————————————————————————————————————————————————————
// Cookie Helpers
// ——————————————————————————————————————————————————————————
function setCookie(name, value, hours) {
    const expires = new Date(Date.now() + hours * 3600000).toUTCString();
    document.cookie = `${name}=${encodeURIComponent(JSON.stringify(value))};expires=${expires};path=/`;
}

function getCookie(name) {
    const match = document.cookie
        .split(';')
        .map(c => c.trim())
        .find(c => c.startsWith(name + '='));
    if (!match) return null;
    return JSON.parse(decodeURIComponent(match.substring(name.length + 1)));
}

// ——————————————————————————————————————————————————————————
// Hard-coded Product Catalog
// ——————————————————————————————————————————————————————————
const products = {

    Trailmaster: { id: 1, name: "OT Trailmaster", price: 49999 },
    Trek: { id: 2, name: "OT Trek", price: 89995 },
    Lynx: { id: 3, name: "OT Lynx", price: 29995 },
    Vector: { id: 4, name: "OT Vector", price: 89999 }

};

// ——————————————————————————————————————————————————————————
// Update Cart Badge with Total Quantity
// ——————————————————————————————————————————————————————————
function updateCartBadge() {
    const cart = getCookie("cart") || [];
    const totalQty = cart.reduce((sum, item) => sum + (item.quantity || 0), 0);
    const badge = document.getElementById("cartCountBadge");
    if (badge) badge.innerText = totalQty;
}

// ——————————————————————————————————————————————————————————
// Add or Increase Item in Cart + Fly “+1” Animation
// ——————————————————————————————————————————————————————————
function addToCart(evt, productKey) {
    evt.preventDefault();

    const product = products[productKey];
    if (!product) {
        console.error("Unknown product:", productKey);
        return;
    }

    // 1) Fly animation
    const btn = evt.currentTarget;
    const btnRect = btn.getBoundingClientRect();
    const cartBadge = document.getElementById("cartCountBadge");
    const cartRect = cartBadge.getBoundingClientRect();

    const fly = document.createElement("span");
    fly.className = "fly-number";
    fly.textContent = "+1";
    document.body.appendChild(fly);

    // start just below the button center
    fly.style.left = `${btnRect.left + btnRect.width / 2}px`;
    fly.style.top = `${btnRect.top + btnRect.height}px`;
    fly.getBoundingClientRect(); // force repaint

    const dx = (cartRect.left + cartRect.width / 2) - (btnRect.left + btnRect.width / 2);
    const dy = (cartRect.top + cartRect.height / 2) - (btnRect.top + btnRect.height);

    fly.style.transform = `translate(${dx}px, ${dy}px) scale(0.5)`;
    fly.style.opacity = "0";
    fly.addEventListener("transitionend", () => fly.remove());

    // 2) Cookie-based cart logic
    let cart = getCookie("cart") || [];
    const existing = cart.find(item => item.id === product.id);

    if (existing) {
        existing.quantity++;
    } else {
        cart.push({ ...product, quantity: 1 });
    }

    setCookie("cart", cart, 1);
    updateCartBadge();
}

// ——————————————————————————————————————————————————————————
// Change Quantity or Remove Item
// ——————————————————————————————————————————————————————————
function changeQuantity(id, delta) {
    let cart = getCookie("cart") || [];
    cart = cart
        .map(item => item.id === id ? { ...item, quantity: item.quantity + delta } : item)
        .filter(item => item.quantity > 0);

    setCookie("cart", cart, 1);
    updateCartBadge();
    showCart();
}

// ——————————————————————————————————————————————————————————
// Build and Show Cart Modal
// ——————————————————————————————————————————————————————————
function showCart() {
    const cart = getCookie("cart") || [];
    const body = document.getElementById("cartModalBody");
    if (!body) return;

    if (cart.length === 0) {
        body.innerHTML = "<p>Your shopping cart is empty.</p>";
    } else {
        let html = '<ul class="list-group">';
        cart.forEach(item => {
            const subtotal = (item.price * item.quantity).toLocaleString("sv-SE");
            html += `
  <li class="list-group-item d-flex justify-content-between align-items-center">
    <div>
      <strong>${item.name}</strong>
      <div class="btn-group btn-group-sm ms-2" role="group">
        <button class="btn btn-outline-secondary" onclick="changeQuantity(${item.id}, -1)">−</button>
        <span class="px-2">${item.quantity}</span>
        <button class="btn btn-outline-secondary" onclick="changeQuantity(${item.id}, +1)">+</button>
      </div>
    </div>
    <span class="badge bg-primary">${subtotal} SEK</span>
  </li>`;
        });
        html += "</ul>";

        const total = cart
            .reduce((sum, i) => sum + i.price * i.quantity, 0)
            .toLocaleString("sv-SE");

        html += `
  <div class="d-flex justify-content-between align-items-center mt-3 p-2 border-top">
    <strong>Total</strong>
    <strong>${total} SEK</strong>
  </div>`;
        body.innerHTML = html;
    }

    bootstrap.Modal.getOrCreateInstance(document.getElementById("cartModal")).show();
}

// ——————————————————————————————————————————————————————————
// Init on page load & clean up
// ——————————————————————————————————————————————————————————
document.addEventListener("DOMContentLoaded", updateCartBadge);
document
    .getElementById("cartModal")
    .addEventListener("hidden.bs.modal", () =>
        document.querySelectorAll(".modal-backdrop").forEach(el => el.remove())
    );
