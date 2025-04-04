<?php
header("Content-Type: application/json");

$host = "103.91.190.179";
$dbname = "testdev09";
$username = "testdev09";
$password = "GPkO9ZcTZ0SNq9lSKbSKXn9JhEfuf9Me";

$conn = new mysqli($host, $username, $password, $dbname);

if ($conn->connect_error) {
    die(json_encode(["status" => "error", "message" => "Connection failed"]));
}

if ($_SERVER["REQUEST_METHOD"] === "POST") {
    $data = json_decode(file_get_contents("php://input"), true);

    if (!isset($data["userid"]) || !isset($data["diamond"])) {
        echo json_encode(["status" => "error", "message" => "Missing parameters"]);
        exit;
    }

    $userid = $conn->real_escape_string($data["userid"]);
    $diamond = $conn->real_escape_string($data["diamond"]);
    $query = "Update userdata set diamond = '$diamond' where userid = '$userid';";
    if ($conn->query($query) === TRUE ) {
    echo json_encode(["status" => "success", "message" => "Diamond Update successfully", "id" => $userid]);
    } else {
    echo json_encode(["status" => "error", "message" => "Update failed: " . $conn->error]);
    }
    }

$conn->close();
?>