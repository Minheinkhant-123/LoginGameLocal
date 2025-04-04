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

    $username = $conn->real_escape_string($data["username"]);
    $password = $data["password"];
    $dataid= uniqid();
    $query = "SELECT a.password,a.id,b.logintime FROM users a left join loginhistory b on b.userid = a.id WHERE username = '$username' ;";
    $result = $conn->query($query);

    if ($result->num_rows > 0) {
        $row = $result->fetch_assoc();
        $hashed_password = $row["password"];
        $id = $row["id"];
        $logintime = isset($row["logintime"]) ? (int)$row["logintime"] : 0;
        if ($password== $hashed_password) {
            if($logintime > 0) {
                $query3 = "UPDATE loginhistory SET logincount = logincount + 1 WHERE userid = '$id'";
                $conn->query($query3);
            } else {
                $query3 = "INSERT INTO loginhistory (id, userid,logintime,logincount) VALUES ('$dataid', '$id',NOW(),1)";
                $conn->query($query3);
            }
            echo json_encode(["status" => "success", "message" => "Login successful", "id" => $id]);
        } else {
            echo json_encode(["status" => "error", "message" => "Invalid credentials"]);
        }
    } else {
        echo json_encode(["status" => "error", "message" => "User not found"]);
    }
}

$conn->close();
?>