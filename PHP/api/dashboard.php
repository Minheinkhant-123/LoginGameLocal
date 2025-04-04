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

    if (!isset($data["userid"])) {
        echo json_encode(["status" => "error", "message" => "Missing parameters"]);
        exit;
    }

    $userid = $conn->real_escape_string($data["userid"]);

    // Fetch user data
    $query = "SELECT heart,diamond FROM userdata WHERE userid = '$userid'";
    $result = $conn->query($query);

    if ($result->num_rows > 0) {
        $row = $result->fetch_assoc();
            echo json_encode(["status" => "success", "message" => "Login successful","heart" => $row["heart"], "diamond" => $row["diamond"]]);
    } else {
        echo json_encode(["status" => "error", "message" => "Invalid credentials"]);
    }
} else {
    echo json_encode(["status" => "error", "message" => "User not found"]);
}


$conn->close();
?>