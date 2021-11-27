

function NavMenu() {
    var sidebar = document.querySelector(".sidebar");
    var closeBtn = document.querySelector("#btn");
    //var searchBtn = document.querySelector(".bx-search");

    closeBtn.addEventListener("click", (e) => {
        sidebar.classList.toggle("open");
        menuBtnChange();
    });

    //searchBtn.addEventListener("click", (e) => { // Sidebar open when you click on the search iocn
    //    sidebar.classList.toggle("open");
    //    menuBtnChange(); //calling the function(optional)
    //});

   
    function menuBtnChange() {
        if (sidebar.classList.contains("open")) {
            closeBtn.classList.replace("bx-menu", "bx-menu-alt-right");//replace the icon class
        } else {
            closeBtn.classList.replace("bx-menu-alt-right", "bx-menu");//replace the icon class
        }
    }
}