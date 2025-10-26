// wwwroot/js/css-checker.js
function checkLoadedCSS() {
    const stylesheets = Array.from(document.styleSheets);
    console.log('Loaded CSS files:');
    stylesheets.forEach(sheet => {
        if (sheet.href) {
            console.log('✅ ' + sheet.href);
        }
    });
}

document.addEventListener('DOMContentLoaded', checkLoadedCSS);