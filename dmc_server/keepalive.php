<?php
require_once("init.php");
file_put_contents('./keepalive/' . $token . '.txt', time());
?>