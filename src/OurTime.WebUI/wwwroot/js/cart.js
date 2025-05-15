// ——————————————————————————————————————————————————————————
// Cookie‐helpers
// ——————————————————————————————————————————————————————————
function setCookie(name, value, hours) {
    const date = new Date();
    date.setTime(date.getTime() + hours * 3600000);
    document.cookie = `${name}=${encodeURIComponent(JSON.stringify(value))};expires=${date.toUTCString()};path=/`;
}

function getCookie(name) {
    return document.cookie
        .split(";")
        .map(c => c.trim())
        .find(c => c.startsWith(name + "="))
        ?.substring(name.length + 1)
        ? JSON.parse(decodeURIComponent(document.cookie
            .split(";")
            .map(c => c.trim())
            .find(c => c.startsWith(name + "="))
            .substring(name.length + 1)))
        : null;
}

// ——————————————————————————————————————————————————————————
// Hård‐kodad produktlista
// ——————————————————————————————————————————————————————————
const products = {
    "Trailmaster": { id: 1, name: "OT Trailmaster", price: 49999 },
    "Treck": { id: 2, name: "OT Treck", price: 89995 },
    "Lynx": { id: 3, name: "OT Lynx", price: 29995 }
};

// ——————————————————————————————————————————————————————————
// Uppdatera badge med totala antalet varor
// ——————————————————————————————————————————————————————————
function updateCartBadge() {
    const cart = getCookie("cart") || [];
    const totalQty = cart.reduce((sum, item) => sum + (item.quantity || 0), 0);
    const badge = document.getElementById("cartCountBadge");
    if (badge) badge.innerText = totalQty;
}

// ——————————————————————————————————————————————————————————
// Lägg till/öka vara i kundkorgen
// ——————————————————————————————————————————————————————————
function addToCart(itemKey) {
    const data = products[itemKey];
    if (!data) return console.error("Okänd produkt:", itemKey);

    let cart = getCookie("cart") || [];
    const existing = cart.find(x => x.id === data.id);

    if (existing) {
        existing.quantity = (existing.quantity || 0) + 1;
    } else {
        cart.push({ ...data, quantity: 1 });
    }

    setCookie("cart", cart, 1);
    updateCartBadge();
}

// ——————————————————————————————————————————————————————————
// Ändra kvantitet eller ta bort rad
// ——————————————————————————————————————————————————————————
function changeQuantity(id, delta) {
    let cart = getCookie("cart") || [];
    cart = cart.map(item => {
        if (item.id === id) {
            const newQty = (item.quantity || 0) + delta;
            return { ...item, quantity: newQty };
        }
        return item;
    })
        .filter(item => item.quantity > 0); // tar bort om qty ≤ 0

    setCookie("cart", cart, 1);
    updateCartBadge();
    showCart();  // återrendera modalen
}

// ——————————————————————————————————————————————————————————
// Bygg och visa kundkorgs‐modal
// ——————————————————————————————————————————————————————————
function showCart() {
    const cart = getCookie("cart") || [];
    const body = document.getElementById("cartModalBody");
    if (!body) return;

    if (cart.length === 0) {
        body.innerHTML = '<p>Your shopping cart is empty.</p>';
    } else {
        let html = '<ul class="list-group">';
        cart.forEach(item => {
            const subtotal = (item.price * (item.quantity || 0)).toLocaleString("sv-SE");
            html += `
        <li class="list-group-item d-flex justify-content-between align-items-center">
          <div>
            ${item.name}
            <div class="btn-group btn-group-sm ms-2" role="group">
              <button class="btn btn-outline-secondary" onclick="changeQuantity(${item.id}, -1)">−</button>
              <span class="px-2 align-middle">${item.quantity || 0}</span>
              <button class="btn btn-outline-secondary" onclick="changeQuantity(${item.id}, +1)">+</button>
            </div>
          </div>
          <span class="badge bg-primary">${subtotal} SEK</span>
        </li>`;
        });
        html += "</ul>";

        const total = cart
            .reduce((sum, i) => sum + i.price * (i.quantity || 0), 0)
            .toLocaleString("sv-SE");
        html += `
      <div class="d-flex justify-content-between align-items-center mt-3 p-2 border-top">
        <strong>Sum</strong>
        <strong>${total} SEK</strong>
      </div>`;

        body.innerHTML = html;
    }

    bootstrap.Modal.getOrCreateInstance(cartModalEl).show();

}

// ——————————————————————————————————————————————————————————
// Initiera badge när sidan laddats
// ——————————————————————————————————————————————————————————
document.addEventListener("DOMContentLoaded", updateCartBadge);

// När modalen göms – ta bort ev. kvarvarande backdrop
const cartModalEl = document.getElementById('cartModal');
cartModalEl.addEventListener('hidden.bs.modal', () => {
    document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
});
