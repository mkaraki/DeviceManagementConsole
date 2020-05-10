var now = new Date();
var unixnow = Math.floor(now.getTime() / 1000);

function addPropTable(p, v) {
    var tbl = document.getElementById('proptable');
    var tr = document.createElement('tr');

    var tdp = document.createElement('td');
    var tdv = document.createElement('td');

    tdp.innerText = p;
    tdv.innerText = v;

    tr.appendChild(tdp);
    tr.appendChild(tdv);

    tbl.appendChild(tr);
}

function statusIdToString(id) {
    switch (id) {
        case -1:
            return "Client Reported unexpected error";

        case 0:
            return "Running";

        case 1:
            return "User tried to close DMC client";

        case 2:
            return "Operating System shutting down";

        case 3:
            return "Client stopped";

        default:
            return "Unexpected information received from client";
    }
}

function getTimeStringFromSecs(sec) {
    var date = new Date(sec * 1000);

    var y = date.getUTCFullYear() - 1970;
    var m = date.getUTCMonth();
    if (m < 10) m = '0' + m;
    var d = date.getUTCDate() - 1;
    if (d < 10) d = '0' + d;

    var h = date.getUTCHours();
    if (h < 10) h = '0' + h;
    var i = date.getUTCMinutes();
    if (i < 10) i = '0' + i;
    var s = date.getUTCSeconds();
    if (s < 10) s = '0' + s;

    return `${y}/${m}/${d} ${h}:${i}:${s}`;
}

window.onload = function () {
    document.getElementById('devname').innerText = devname;

    if (katime == -1)
        addPropTable('Keepalive', 'Disabled');
    else {
        var ktimeobj = new Date(katime * 1000);
        var datestr = ktimeobj.getFullYear() + '/' + (ktimeobj.getMonth() + 1) + '/' + ktimeobj.getDate();

        var h = ktimeobj.getHours();
        var m = ktimeobj.getMinutes();
        var s = ktimeobj.getSeconds();
        if (h < 10) h = '0' + h;
        if (m < 10) m = '0' + m;
        if (s < 10) s = '0' + s;

        var timestr = '(' + datestr + ' ' + h + ':' + m + ':' + s + ')';

        if (unixnow - katime > 30)
            addPropTable('Status', 'Lost ' + timestr);
        else
            addPropTable('Status', 'Online ' + timestr);
    }

    if (data === null) {
        addPropTable('Device Monitor', 'Disabled');
    }
    else {
        addPropTable('Computer Status', statusIdToString(data.status));

        // Device
        addPropTable('Uptime', getTimeStringFromSecs(data.performance.uptime));
        addPropTable('CPU Processer Time', data.performance.cpuPerc + '%');
        addPropTable('RAM Available', data.performance.ramMb.toLocaleString() + 'MiB');

        // OS
        addPropTable('OS', data.os.name);
        addPropTable('OS Version', data.os.version);

        // Apps
        addPropTable('DMC Client Version', data.clientVersion);
        addPropTable('Running Process', data.processes.length);
        addPropTable('Services', data.services.length);
    }
};
