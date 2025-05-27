(function () {
    // —— Cookie helpers ——
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

    let productsById = {};

    // —— Load catalog from API ——
    async function loadProducts() {
        try {
            const res = await fetch('/api/products');
            const list = await res.json();
            list.forEach(p => { productsById[p.id] = p; });
        } catch (e) {
            console.error('Could not load products', e);
        }
    }

    // —— Cart badge updater ——
    function updateCartBadge() {
        const cart = getCookie('cart') || [];
        const total = cart.reduce((sum, i) => sum + (i.quantity || 0), 0);
        document.getElementById('cartCountBadge').innerText = total;
    }

    // —— Add one item ——
    function addToCart(evt, productId) {
        evt.preventDefault();
        const prod = productsById[productId];
        if (!prod) return console.error('Unknown product', productId);

        // +1 fly-animation
        const btn = evt.currentTarget;
        const badge = document.getElementById('cartCountBadge');
        const fly = document.createElement('span');
        fly.className = 'fly-number';
        fly.textContent = '+1';
        document.body.appendChild(fly);
        const b = btn.getBoundingClientRect(), c = badge.getBoundingClientRect();
        fly.style.left = `${b.left + b.width / 2}px`;
        fly.style.top = `${b.top + b.height}px`;
        fly.getBoundingClientRect(); // repaint
        fly.style.transform = `translate(${c.left + c.width / 2 - (b.left + b.width / 2)}px,`
            + `${c.top + c.height / 2 - (b.top + b.height)}px) scale(0.5)`;
        fly.style.opacity = '0';
        fly.addEventListener('transitionend', () => fly.remove());

        // uppdatera cookie
        const cart = getCookie('cart') || [];
        const found = cart.find(i => i.id === prod.id);
        if (found) found.quantity++;
        else cart.push({ id: prod.id, name: prod.name, price: prod.price, quantity: 1 });

        setCookie('cart', cart, 1);
        updateCartBadge();
    }

    // —— Change quantity (+/–) ——
    function changeQuantity(id, delta) {
        let cart = getCookie('cart') || [];
        cart = cart
            .map(i => i.id === id ? { ...i, quantity: i.quantity + delta } : i)
            .filter(i => i.quantity > 0);
        setCookie('cart', cart, 1);
        updateCartBadge();
        showCart();
    }

    // —— Render & show modal ——
    function showCart() {
        const cart = getCookie('cart') || [];
        const body = document.getElementById('cartModalBody');
        if (!body) return;

        if (cart.length === 0) {
            body.innerHTML = '<p>Din kundvagn är tom.</p>';
        } else {
            let html = '<ul class="list-group">';
            cart.forEach(i => {
                const sub = (i.price * i.quantity)
                    .toLocaleString('sv-SE');
                html += `
<li class="list-group-item d-flex justify-content-between align-items-center">
  <div>
    <strong>${i.name}</strong>
    <div class="btn-group btn-group-sm ms-2" role="group">
      <button class="btn btn-outline-secondary" onclick="changeQuantity(${i.id}, -1)">−</button>
      <span class="px-2">${i.quantity}</span>
      <button class="btn btn-outline-secondary" onclick="changeQuantity(${i.id}, +1)">+</button>
    </div>
  </div>
  <span class="badge bg-primary">${sub} SEK</span>
</li>`;
            });
            html += '</ul>';

            const total = cart
                .reduce((sum, i) => sum + i.price * i.quantity, 0)
                .toLocaleString('sv-SE');
            html += `
<div class="d-flex justify-content-between align-items-center mt-3 p-2 border-top">
  <strong>Total</strong>
  <strong>${total} SEK</strong>
</div>`;

            body.innerHTML = html;
        }

        bootstrap.Modal.getOrCreateInstance(
            document.getElementById('cartModal')
        ).show();
    }

    // —— Init ——
    document.addEventListener('DOMContentLoaded', async () => {
        await loadProducts();
        updateCartBadge();
        document.getElementById('cartModal')
            .addEventListener('hidden.bs.modal', () =>
                document.querySelectorAll('.modal-backdrop').forEach(el => el.remove())
            );
    });

    // expose for onclick handlers
    window.addToCart = addToCart;
    window.changeQuantity = changeQuantity;
    window.showCart = showCart;
})();
