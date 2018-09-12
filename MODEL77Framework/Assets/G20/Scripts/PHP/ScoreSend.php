<?php

header("Conect-Type: application/json; charset=UTF-8");
header("X-content-Type-Options: nosniff");


//tryの中に通常処理
try{
    $dsn = 'mysql:dbname=WT;host=localhost';
	$user = 'root';
	$password = '';
	$con =new PDO($dsn,$user,$password);



    $query="select * from score where (date = " . $_POST['date'] . " or date = 9999) and (difficulty=" . $_POST['difficulty'] .  ") order by score desc limit 1";
    //$query="select * from score where (date = 910 or date = 9999) and (difficulty=1) order by score desc limit 1";

    
    $stmt = $con->prepare($query);
    $stmt -> execute();
	$result = $stmt -> fetchAll(PDO::FETCH_ASSOC);
    

	    $json=json_encode($result);
    
    
}
//catchの中に例外処理
catch(PDOEException $Exception){
    die('接続エラー'.$Exception->getMessage());
}

//var_dump($result);

echo $json;
?>