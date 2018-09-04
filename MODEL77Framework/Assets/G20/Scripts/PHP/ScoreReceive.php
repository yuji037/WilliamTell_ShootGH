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
//insert into score values(' $userinfo ' , $date , $score , $ID , $difficulty);

    $query=  "insert into score values(' ";
	$query.= $_POST['userinfo'];	
	$query.= " ', ";
 	$query.= $_POST['month'] . $_POST['day']; 		
	$query.= " , "; 
	$query.= $_POST['score']; 	
	$query.= " , "; 
	$query.= $_POST['ID']; 		
	$query.= " , "; 
	$query.= $_POST['difficulty'] . ")";
	
    $stmt = $con->prepare($query);
    $stmt -> execute();
	$result = $stmt -> fetchAll(PDO::FETCH_ASSOC);
    
   

	echo "userinfo:".$_POST['userinfo']."\ndate:".$_POST['date']."\nscore:".$_POST['score']."\nID:".$_POST['ID']."\ndifficulty:".$_POST['difficulty'];

    }
//catchの中に例外処理
catch(PDOEException $Exception){
    die('接続エラー'.$Exception->getMessage());
}

//var_dump($result);

?>