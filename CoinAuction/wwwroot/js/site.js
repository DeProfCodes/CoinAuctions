//Count down timer
function Timer(countdownStr)
{
    if (countdownStr === "active")
    {
        document.getElementById("timer").innerHTML = "Auction Is Active";
    }
    else if (countdownStr === "n/a")
    {
        document.getElementById("timer").innerHTML = "Auction has not been scheduled yet";
    }
    else
    {
        var countDownDate = new Date(countdownStr).getTime();

        var x = setInterval(function ()
        {
            var now = new Date().getTime();
            var distance = countDownDate - now;

            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            document.getElementById("timer").innerHTML = days + ":" + hours + ":" + minutes + ":" + seconds;

            if (distance < 0) {
                clearInterval(x);
                document.getElementById("timer").innerHTML = "Auction Is Active";
            }
        }, 1000);
    }
}

$(function ()
{
    $('div[onload]').trigger('onload');
});

function CoinsLimit(maxCoins)
{
    var bidCoins = parseInt($("#bidCoins").val());
    if (bidCoins > parseInt(maxCoins))
    {
        $("#coinsError").text("Bid coins exceed max");
        return false;
    }
    return true;
}

function SweetConfirmCancel(id, command, action)
{
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: command + ' Bid'
    }).then((result) =>
        {
            if (result.isConfirmed)
            {
                window.location.href = "/Dashboard/"+action+"/"+id;
            }
    })
}

$('#btnSubmit').click(function () {
    $('.spinner').css('display', 'block');
});


function SweetConfirmAuction(command, message,controller,action, id) {
    Swal.fire({
        title: 'Are you sure?',
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: command
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = "/" + controller +"/" + action + "/" + id;
        }
    })
}

function SweetConfirmStartNowAuction() {
    Swal.fire({
        title: 'Are you sure?',
        text: "This will stop any active auction and create a new one!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Start Auction'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = "/Auctions/StartAuctionNow";
        }
    })
}

function SweetDone() {
    Swal.fire({
        title: 'Good',
        text: "Perfect",
        icon: 'success'
    })
}

function PasswordsMisMatch()
{
    if ($("#password").val() != $("#confpass").val())
    {
        $("#confirmPassword").text("Passwords do not match!");
        return false;
    }
    return true;
}


function myFunction() {
    var x = document.getElementById("myTopnav");

    if (x.className === "topnav") {
        x.className += " responsive";
    }
    else {
        x.className = "topnav";
    }
}