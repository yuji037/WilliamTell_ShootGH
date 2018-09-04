<?php

header("Conect-Type: application/json; charset=UTF-8");
header("X-content-Type-Options: nosniff");


//tryの中に通常処理
try{
$dsn = 'mysql:dbname=WT;host=localhost';
	$user = 'root';
	$password = '';
	$con =new PDO($dsn,$user,$password);

$query="select max(ID) as id from score";
	$stmt = $con->prepare($query);
   	$stmt -> execute();
    $result = $stmt -> fetch(PDO::FETCH_ASSOC);

    $num = 0;

	if($result!=null){
	    $num=$result["id"];
	}	

    $num+=1;

}
catch(PDOEException $Exception){
    die('接続エラー'.$Exception->getMessage());
}

echo $num;
?>