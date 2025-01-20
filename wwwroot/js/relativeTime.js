function updateRelativeTime() {
    $('.relative-time').each(function () {
        var datetime = $(this).data('datetime');
        var timeAgo = moment.utc(datetime).fromNow();
        $(this).text(timeAgo);
    });
}

$(function () {
    updateRelativeTime();
    setInterval(updateRelativeTime, 60000); // Update every minute
});