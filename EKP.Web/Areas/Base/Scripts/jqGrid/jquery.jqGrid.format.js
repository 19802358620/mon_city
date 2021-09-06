function utcDateFormatter(cellvalue, options, rowObject) {
    if (cellvalue) {
        return moment(cellvalue).format('l');
    } else {
        return '';
    }
}

function utcDateTimeFormatter(cellvalue, options, rowObject) {
    if (cellvalue) {
        return moment(cellvalue).format('YYYY/MM/DD HH:mm');
    } else {
        return '';
    }
}