<?php

if ($_SERVER['REQUEST_METHOD'] != 'POST') 
{
    die ("Not Supported");
}

$taskfile = './task/' . $_POST['target'] . '.json';
$taskdata = [ 'type' => $_POST['type'], 
'options' => explode(';', $_POST['options'])];

file_put_contents($taskfile, json_encode($taskdata));
?>
<!DOCTYPE html>
<html>

<head>
    <title>Device Management Console</title>
</head>

<body>
    Queued<br />
    <a href="<?php echo $_POST['from'] ?>">Back</a>
</body>

</html>