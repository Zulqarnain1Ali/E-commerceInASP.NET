// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Add to wwwroot/js/site.js
document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".cart-btn");
        if (!btn) return;

        const prodId = btn.getAttribute("data-prod-id");
        const iconSpan = btn.querySelector(".btn-icon");
        const textSpan = btn.querySelector(".btn-text");

        btn.disabled = true;
        iconSpan.innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`;
        textSpan.textContent = "Adding...";

        const formData = new FormData();
        formData.append("Prod_id", prodId);

        fetch("/Customer/addToCart", {
            method: "POST",
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            if (data.redirectUrl) {
                window.location.href = data.redirectUrl;
                return;
            }

            if (data.success) {
                btn.classList.remove("btn-dark");
                btn.classList.add("btn-success");
                iconSpan.textContent = "✅";
                textSpan.textContent = "Added!";

                setTimeout(() => {
                    btn.classList.remove("btn-success");
                    btn.classList.add("btn-dark");
                    iconSpan.textContent = "🛒";
                    textSpan.textContent = "Add to Cart";
                    btn.disabled = false;
                }, 2000);
            }
        })
        .catch(error => {
            console.error("Cart error:", error);
            btn.classList.remove("btn-dark");
            btn.classList.add("btn-danger");
            iconSpan.textContent = "⚠️";
            textSpan.textContent = "Error";

            setTimeout(() => {
                btn.classList.remove("btn-danger");
                btn.classList.add("btn-dark");
                iconSpan.textContent = "🛒";
                textSpan.textContent = "Add to Cart";
                btn.disabled = false;
            }, 2000);
        });
    });
});