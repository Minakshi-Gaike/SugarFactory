/* =========================================
   VEARIUM WEBSITE INTRO
   ========================================= */

window.addEventListener("load", function () {

    const intro = document.getElementById("veariumIntro");

    if (!intro) {
        return;
    }

    /*
       Intro duration:
       Bags animation
       +
       Company name
       +
       Exit
    */

    setTimeout(function () {

        intro.classList.add("intro-hide");

    }, 6200);

});