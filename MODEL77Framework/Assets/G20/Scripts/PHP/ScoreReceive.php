<?php

header("Conect-Type: application/json; charset=UTF-8");
header("X-content-Type-Options: nosniff");
$userinfo=' ';
$date=' ';
$score=' ';
$ID=' ';
$difficulty=' ';

//tryの中に通常処理
try{
if(isset( $_POST['userinfo'])
 &&isset( $_POST['date'])
 &&isset( $_POST['score'])
 &&isset( $_POST['ID'])
 &&isset( $_POST['difficulty'])
 ){
	$userinfo=$_POST['userinfo'];
	$date=$_POST['date'];
	$score=$_POST['score'];
	$ID=$_POST['ID'];
	$difficulty=$_POST['difficulty'];
	}

	$dsn = 'mysql:dbname=WT;host=localhost';
	$user = 'root';
	$password = '';
	$con =new PDO($dsn,$user,$password);

    $query=  "insert into score values('";
	$query.= $_POST['userinfo'];	
	$query.= "', ";
 	$query.= $_POST['date']; 		
	$query.= " , "; 
	$query.= $_POST['score']; 	
	$query.= " , "; 
	$query.= $_POST['ID']; 		
	$query.= " , "; 
	$query.= $_POST['difficulty'] . ")";
	
	//$query="insert into score values('init' , 9999 , 0 , 0 , 1)";
	
    $stmt = $con->prepare($query);
    $stmt -> execute();
	$result = $stmt -> fetchAll(PDO::FETCH_ASSOC);
    
   

	echo "データ送信完了";

    }
//catchの中に例外処理
catch(PDOEException $Exception){
    die('接続エラー'.$Exception->getMessage());
}

//var_dump($result);

?>