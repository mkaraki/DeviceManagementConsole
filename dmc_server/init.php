<?php
$headers = apache_request_headers();
if (!isset($headers["Authorization"])){
    error_log("!!!!!----- No allowed access found. -----!!!!!");
    header("HTTP/1.0 403 Forbidden");
    die("Access Denied");
}
$token = explode(' ', $headers["Authorization"])[1];
?>
