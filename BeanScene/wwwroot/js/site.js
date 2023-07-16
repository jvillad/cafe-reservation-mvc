// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.







// https://stackoverflow.com/questions/73867054/calculate-time-intervals-javascript

function updateEndTimeOptions() {
    // selected start time
    const startTime = new Date(this.value);
    // Calculate maximum end time by adding 3 hours to the start time
    // 3 hrs maximum allowed sitting time
    // * 60 seconds in a minute
    // * 60 minutes in an hour
    // * 1000 milliseconds in a second
    // = 10800000 milliseconds
    const maxEndTime = new Date(startTime.getTime() + 3 * 60 * 60 * 1000);
    // Reservation_EndTimeId select element
    const endTimeSelect = document.getElementById('Reservation_EndTimeId');
    
    for (let i = 0; i < endTimeSelect.options.length; i++) {
        // time associated with teh current option and create new object
        const optionTime = new Date(endTimeSelect.options[i].value);
        // range checking
        if (optionTime < startTime || optionTime > maxEndTime) {
            // disable the current option
            endTimeSelect.options[i].disabled = true;
        } else {
            // Otherwise
            endTimeSelect.options[i].disabled = false;
        }
    }
}

document.getElementById('Reservation_StartTimeId').addEventListener('change', updateEndTimeOptions);

updateEndTimeOptions.call(document.getElementById('Reservation_StartTimeId'));

