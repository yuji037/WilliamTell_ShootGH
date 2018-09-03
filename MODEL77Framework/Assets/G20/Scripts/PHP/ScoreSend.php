<?php

header("Conect-Type: application/json; charset=UTF-8");
header("X-content-Type-Options: nosniff");

//tryの中に通常処理
try{
$dsn = 'mysql:dbname=WT;host=localhost';
	$user = 'root';
	$password = '';
	$con =new PDO($dsn,$user,$password);

    $query="select * from score";
    $stmt = $con->prepare($query);
    $stmt -> execute();
	$result = $stmt -> fetchAll(PDO::FETCH_ASSOC);
    
    $score_array = array();

    foreach($result as $row){
		$score_array[] =$row["userinfo"];
    	$score_array[] =$row["date"];
		$score_array[] =$row["score"];    	
		$score_array[] =$row["ID"];    	
		$score_array[] =$row["difficulty"];    	
	}
		//配列の文字列結合のための関数
	$score=implode(",",$score_array);

    
    
    }
//catchの中に例外処理
catch(PDOEException $Exception){
    die('接続エラー'.$Exception->getMessage());
}

//var_dump($result);

echo $score;
?>