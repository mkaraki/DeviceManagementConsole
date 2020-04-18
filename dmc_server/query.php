<?php
if (!isset($_GET['q']))
{
    die('no device');
}

$devicet = base64_encode($_GET['q']);
$report_file = './report/' . $devicet . '.json';
$ka_file = './keepalive/' . $devicet . '.txt';

$report_av = file_exists($report_file);
$ka_av = file_exists($ka_file);

if ($ka_av)
{
    $timestamp = intval(file_get_contents($ka_file));
}

if ($report_av)
{
    $report_json = file_get_contents($report_file);
    $report = json_decode($report_json, true);
}
?>
<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <title>Query - Device Management Console</title>
    <script>
        var devname = '<?php echo $_GET['q'] ?>';
        var katime = <?php echo $ka_av ?$timestamp: -1?>;
        var data = <?php echo $report_av ?$report_json: 'null'; ?>;
    </script>
    <script src="query.js"></script>

    <link rel="stylesheet" href="query.css">
</head>

<body>
    <h1>Device Management Console</h1>
    <h3 id="devname"></h3>
    <div class="section">
        <h5>Queue Task</h5>
        <form action="queuetask.php" method="POST">
            <div>
                <label for="type">Type</label>
                <select id="type" name="type">
                    <option value="tell">Tell Message</option>
                    <option value="lock">Lock Computer</option>
                    <option value="shutdown">Shutdown Computer</option>
                    <option value="reboot">Reboot Computer</option>
                    <option value="stop">Stop &quot;Device Management Console - Client&quot;</option>
                </select>
            </div>
            <div>
                <label for="options">Option</label>
                <input type="text" id="options" name="options">
            </div>
            <input type="hidden" name="target" value="<?php echo $devicet; ?>">
            <input type="hidden" name="from" value="<?php echo $_SERVER['REQUEST_URI']; ?>">
            <input type="submit" value="Queue">
        </form>
    </div>
    <div class="section">
        <h5>Status</h5>
        <table>
            <thead>
                <tr>
                    <th>Property</th>
                    <th>Value</th>
                </tr>
            </thead>
            <tbody id="proptable">

            </tbody>
        </table>
    </div>
</body>

</html>