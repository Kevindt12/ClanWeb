'use strict';







//--------------- Jquery Ajax Responses --------------------//
$(function () {

    //
    // WHERE: Forums/Messages : The up and down button for the voting on messages
    // When clicked on a vote manager arrow up or down
    $(document).on('click', '.votes_manager', function () {

        // Getting the number element
        var reputationSystemElem = $(this).parent();
        var numberElem = reputationSystemElem.children('.displayer');
        var postUrl = "";

        // Getting the ids for the method parameters
        var imessageId = reputationSystemElem.children('#messageid')[0].attributes.value.value;
        var ithreadId = getParameterByName('threadId');
        var igroupId = getParameterByName('groupId');

        // Sees with button was pressed and adds or decreses the value based on the button
        if ($(this).hasClass('increase')) {
            postUrl = "/forum/MessageReputationUp";
        }
        else {
            postUrl = "/forum/MessageReputationDown";
        }

        // Creating the ajax and sending the data back to the server
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: JSON.stringify({ messageId: imessageId }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                numberElem.text(data.reputation);
            }
        });

    });



    //
    // WHERE: Forum/Thread - When a user presses the subscribe button
    $(document).on('click', '#btnsubscribe', function () {

        // Getting base values
        var ithreadId = getParameterByName('threadId');
        var methodName = '';
        var elem = $(this);


        if (elem.text() === 'Subscribe') {
            methodName = "/forum/AddThreadToSubscriptions";
        } else {
            methodName = "/forum/RemoveThreadFronSubscriptions";
        }

        // Remove css elements
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-default");


        // Creating the ajax and sending the data back to the server
        $.ajax({
            url: methodName,
            type: 'POST',
            data: JSON.stringify({ threadId: ithreadId }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.subscribed === true) {
                    elem.text('Unsubscribe');
                    // Chaning some css class elements 
                    $(this).removeClass("btn-default");
                    $(this).addClass("btn-success");
                } else {
                    elem.text('Subscribe');
                    // Chaning some css class elements 
                    $(this).removeClass("btn-danger");
                    $(this).addClass("btn-default");
                }
            }
        });

    });

    //
    // WHERE: Front Page - When clicking on a event
    // It will show a dialog of a event in full details
    $(document).on('click', '.eventlisting', function () {

        // Getting the event Id of the clicked event
        var ieventId = $(this).parent().children('#eventid')[0].attributes.value.value;

        // Creating the ajax and sending the data back to the server
        $.ajax({
            url: "/home/FullEventDetails",
            type: 'GET',
            data: { eventId: ieventId },
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                // Showing the dialog
                $('#eventmodal').modal('show');

                // Getting the elements
                $('#eventName').text(data.model.EventName);
                $('#eventTitle').text("Event - " + data.model.EventName);
                $('#gameName').text(data.model.Game);
                $('#locationName').text(data.model.Location);
                $('#description').html(data.model.Description);
                $('#createdBy').text(data.model.CreatedBy);
                $('#eventDateTime').text(data.model.EventDateAndTime);
                $("#eventImage").attr("src", data.model.ImageLocation);

            }
        });
    });


    // WHERE: Front Page - When clicking on a event
    // It will show a dialog of a event in full details
    $(document).on('click', '.approve-user', function (event) {
        var self = this;

        var $button = $(self);
        var $row = $button.closest('tr');
        var iuserId = $button.data('userid');

        console.log("Request started");

        /**
         * This are the sucsess function if a user has been added to the clanw
         * */
        function succsess() {

            // Change the background a light green
            $row.css('background-color', '#163008');

            // We will also disable all the buttons so you cant do this again
            $row.find('button').each(function () {
                $(this).prop('disabled', 'true');
            });
        }

        // THe ajax call
        $.ajax({
            url: "/Admin/ApproveUser",
            type: 'POST',
            data: { userId: iuserId },
            contentType: 'application/json; charset=utf-8',
            success: succsess()
        });

    });



    // WHERE: Front Page - When clicking on a event
    // It will show a dialog of a event in full details
    $(document).on('click', '.disapprove-user', function (event) {
        var self = this;

        var $button = $(self);
        var $row = $button.closest('tr');
        var iuserId = $button.data('userid');

        

        /**
         * This will delete the row and not show the user anymore
         * */
        function succsess() {

            // Delete the row
            $row.remove();
        }


        // The ajax call
        $.ajax({
            url: "/Admin/DispproveUser",
            type: 'POST',
            data: { userId: iuserId },
            contentType: 'application/json; charset=utf-8',
            success: succsess()
        });

    });



    // WHERE: Front Page - When clicking on a event
    // It will show a dialog of a event in full details
    $(document).on('click', '.confirm-delete', function () {
        return confirm('Are you sure you wish to delete this?');
    });



    // This will trigger when a Ajax call has failed. This will make sure we can always debug a error
    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {

        // We console log all of it so we can see it in the console and debug where it happend
        console.log(event);
        console.log(jqxhr);
        console.log(settings);
        console.log(thrownError);
    });


});



// ------------------------- Jquery Big UI -----------------//
$(function () {


    //
    // WHERE: Admin/CreateEvent - Shows a date Time picker
    // Shows a date time picker for that event
    $('#eventDateTimePicker').datetimepicker({

        // Changing it so it will show best for europe
        locale: 'en-gb',

        // Setting today as minimum date
        minDate: new Date(),

        // Setting up the format - ALERT Dont remove this it will break it
        format: 'YYYY-MM-DD HH:mm:ss',
        showClear: true,
        toolbarPlacement: 'top'
    });

    //
    // WHERE: Admin/CreateEvent - Shows a date Time picker
    // Shows a date time picker for that event
    $('#newsDateTimePicker').datetimepicker({

        // Changing it so it will show best for europe
        locale: 'en-gb',

        // Setting up the format - ALERT Dont remove this it will break it
        format: 'YYYY-MM-DD HH:mm:ss',
        showClear: true,
        toolbarPlacement: 'top'
    });


    $(".members-img").hover({

        // Try and create a render with there name on the picture
        // There name
        


    });


    // This will open the voice coms side bar
    $('#showVoiceSidebar').click(function () {

        var self = this;
        var $voiceSidebar = $('.voice-sidebar');
        var width = '250px';

        if ($voiceSidebar.hasClass('.active')) {
            closeVoiceSidebar();

        } else {
            openVoiceSidebar();
        }

    });


    // This will close the sidebar panel
    function closeVoiceSidebar() {
        var $voiceSidebar = $('.voice-sidebar');
        var $button = $('#showVoiceSidebar');

        // If the sidebar is closed then add the class and open it
        $voiceSidebar.css('width', '0px');

        // adding the class to state that this is open
        $voiceSidebar.removeClass('.active');

        // Moving the button. And changing the text so its clear that you can open something here
        $button.css('right', '0px');
        $button.text('<');
    }


    // This will open the sidebar panel
    function openVoiceSidebar() {
        var $voiceSidebar = $('.voice-sidebar');
        var $button = $('#showVoiceSidebar');


        // If the sidebar is open then close it
        $voiceSidebar.css('width', '250px');

        // Removing the class
        $voiceSidebar.addClass('.active');

        // Moving the button. And chaning the text so its logic that this means close for the next time you click it 
        $button.css('right', '250px');
        $button.text('>');
    }




    //// Activates the carousel on the fron page
    //$('#FrontPageCarousel').carousel({
    //    interval: 2000
    //});


});






// ------------------------- Jquery Small UI -----------------//
$(function () {

    // Sets the css classes in the forum thread For the first row top black bar
    $('.forumgrouprow').first().css('border-top-style', 'solid');
    $('.forumgrouprow').first().css('border-top-color', '#222');
    $('.forumgrouprow').first().css('border-top-width', '1px');

    /*border-top-style:solid;
    border-top-color:#111;
    border-top-width:1px;*/

    // 
    // WHERE: Forums/Thread - When hovering over the subscirbe button
    // Editing the looks of the subscribe button
    $('#btnsubscribe').mouseover(function () {

        // If it has Unsubscribe
        if ($(this).text() !== 'Subscribe') {
            // Change the text of the element
            $(this).text('Unsubscribe')

            // Changing the css of the element so we can change the color
            $(this).removeClass("btn-success");
            $(this).addClass("btn-danger");
        }
    });

    // 
    // WHERE: Forums/Thread - When hovering over the subscirbe button
    // It will change the color in css classes how the button looks like
    $('#btnsubscribe').mouseleave(function () {

        // If it has Unsubscribe
        if ($(this).text() !== 'Subscribe') {
            // Change the text of the element
            $(this).text('Subscribed')

            // Changing the css of the element so we can change the color
            $(this).removeClass("btn-danger");
            $(this).addClass("btn-success");
        }
    });



   

});




// ------------------------- Javascript Small UI -----------------//







$(function () {

    $(document).on('click', '.votes_manager', function () {
        var elem1 = $(this).parent().children('.increase');
        var elem2 = $(this).parent().children('.decrease');
        elem1.prop('disabled', true);
        elem2.prop('disabled', true);
    });

});


//// Sets the datePicker for all textboxes with .datepicker
//$(function () {
//    // Gets the elements and sets the datepicker to them
//    $('#datetimepicker').datetimepicker({

//        // Changing it so it will show best for europe
//        locale: 'en-gb',

//        // Setting today as minimum date
//        minDate: new Date()
//    });
//});


$(function () {
    $("#dialog").dialog({
        autoOpen: false
    });
});




function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}


function testBeforePost() {
    var test = tinyMCE.activeEditor.getContent({ format: 'html' });


}