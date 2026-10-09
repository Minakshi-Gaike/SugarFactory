
document.addEventListener("DOMContentLoaded", function () {

    const intro = document.getElementById("veariumIntro");

    if (!intro) return;

    const introKey = "VeariumIntroShown";

    // Already shown in this browser tab
    if (sessionStorage.getItem(introKey) === "true") {
        intro.remove();
        return;
    }

    // Start animation
    intro.classList.add("intro-playing");

    // Images disappear after 3.3 seconds
    setTimeout(function () {
        intro.classList.add("show-brand");
    }, 3900);

    // Remove intro after logo animation
    setTimeout(function () {
        intro.classList.add("intro-hide");

        setTimeout(function () {
            intro.remove();
        }, 800);

    }, 6200);

    sessionStorage.setItem(introKey, "true");
});
