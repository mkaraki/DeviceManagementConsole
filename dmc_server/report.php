<?php
require_once("init.php");
$data_json = file_get_contents('php://input');
file_put_contents('report/' . $token . '.json', $data_json);
?>
