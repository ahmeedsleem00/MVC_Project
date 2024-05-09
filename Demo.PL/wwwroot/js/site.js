// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var SearchInput = Document.getelementbyId("SearchInput");

SearchInput.AddEventListener("Keyup", function () {

    // Creating Our XMLHttpRequest Object 

    let xhr = new XMLHttpRequest();

    // Making Our Connection  

    let url = 'https://localhost:44359/Employee/Index';

    xhr.open("GET", url, true);

    // Function Execute After Request Is Successful 
    xhr.onreadystatechange = function ()
    {

        if (this.readyState == 4 && this.status == 200)
        {
            console.log(this.responseText);
        }
    }

    // Sending our request 

    xhr.send();
})