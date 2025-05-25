// confetti.js – körs vid sidladdning på Home (Index)

// Ladda in canvas-confetti via CDN
const script = document.createElement("script");
script.src = "https://cdn.jsdelivr.net/npm/canvas-confetti@1.5.1/dist/confetti.browser.min.js";
script.onload = function () {
    // Starta konfettieffekt
function launchConfetti() {
        confetti({
            particleCount: 200,
            spread: 160,
            startVelocity: 45,
            origin: { y: 0.2 }
        });
    }

// Första skottet direkt
launchConfetti();

// Andra skottet efter 1,5 sekunder
setTimeout(() => {
    launchConfetti();
}, 1500);

// Tredje (sista) skottet efter 3 sekunder
setTimeout(() => {
    launchConfetti();
}, 3000);



    // Skapa lanseringstexten
    const text = document.createElement("div");
    text.innerText = "Grand Reveal – OT Watches!";
    text.style.position = "fixed";
    text.style.top = "50%";
    text.style.left = "50%";
    text.style.transform = "translate(-50%, -50%)";
    text.style.fontSize = "2.5rem";
    text.style.color = "#222";
    text.style.background = "rgba(255,255,255,0.85)";
    text.style.padding = "1rem 2rem";
    text.style.borderRadius = "10px";
    text.style.zIndex = "9999";
    text.style.opacity = "0";
    text.style.transition = "opacity 0.5s ease, transform 0.5s ease";
    document.body.appendChild(text);

    // Animera in och ut texten
    setTimeout(() => {
        text.style.opacity = "1";
        text.style.transform = "translate(-50%, -60%)";
    }, 100);

    setTimeout(() => {
        text.style.opacity = "0";
        text.style.transform = "translate(-50%, -80%)";
    }, 3000);

    setTimeout(() => {
        document.body.removeChild(text);
    }, 4000);
};

// Lägg till scriptet i dokumentet
document.head.appendChild(script);

/*
 * Lägg till följande rad i Index.cshtml:
 *
 * Om du har @section Scripts:
 * @section Scripts {
 *     <script src="~/js/confetti.js" asp-append-version="true"></script>
 * }
 *
 * Om du inte har det, lägg istället detta precis innan </body>:
 * <script src="~/js/confetti.js" asp-append-version="true"></script>
 */
