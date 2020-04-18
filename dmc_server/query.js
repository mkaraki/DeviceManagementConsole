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

window.onload = function () {
    document.getElementById('devname').innerText = devname;

    if (katime == -1)
        addPropTable('Keepalive', 'Disabled');
    else if (unixnow - katime > 30)
        addPropTable('Status', 'Lost');
    else
        addPropTable('Status', 'Online');

    if (data === null) {
        addPropTable('Device Monitor', 'Disabled');
    }
    else {
        // Device
        addPropTable('CPU Processer Time', data.performance.cpuPerc + '%');
        addPropTable('RAM Available', data.performance.ramMb.toLocaleString() + 'MiB');

        // OS
        addPropTable('OS', data.os.name);
        addPropTable('OS Version', data.os.version);

        // Apps
        addPropTable('Running Process', data.processes.length);
        addPropTable('Services', data.services.length);
    }
};