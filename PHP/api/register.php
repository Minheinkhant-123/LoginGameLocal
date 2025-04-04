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

    if (!isset($data["username"]) || !isset($data["password"])) {
        echo json_encode(["status" => "error", "message" => "Missing parameters"]);
        exit;
    }

    $userid = uniqid(); 
    $dataid= uniqid();
    $username = $conn->real_escape_string($data["username"]);
    $password = $conn->real_escape_string($data["password"]);
    $query = "SELECT id FROM users WHERE username = '$username' ;";
    $result = $conn->query($query);

    if ($result->num_rows > 0) {
        $row = $result->fetch_assoc();
        $id = $row["id"];
        if ($id != null) {
            echo json_encode(["status" => "error", "message" => "Username already exists"]);
            exit;
        }
        
    }
    else {
        $query = "INSERT INTO users (id, username, password) VALUES ('$userid', '$username', '$password')";
        $query2 = "INSERT INTO userdata (id, userid) VALUES ('$dataid', '$userid')";
        if ($conn->query($query) === TRUE && $conn->query($query2) === TRUE) {
        echo json_encode(["status" => "success", "message" => "User registered successfully", "id" => $userid]);
        } else {
        echo json_encode(["status" => "error", "message" => "Registration failed: " . $conn->error]);
        }
    }
   
}

$conn->close();
?>