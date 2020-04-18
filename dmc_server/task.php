<?php
require_once('init.php');

$method = $_SERVER['REQUEST_METHOD'];

$taskfile = './task/' . $token . '.json';
$tf_av = file_exists($taskfile);

if ($method == 'DELETE')
{
    if ($tf_av)
    {
        unlink($taskfile);
    }
}
else
{
    if ($tf_av)
    {
        echo file_get_contents($taskfile);
    }
    else
    {
        header("HTTP/1.0 404 Not Found");
    }
}
?>