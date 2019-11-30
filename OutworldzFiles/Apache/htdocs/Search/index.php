<html lang="en-us">
<!-- AGPL 3.0 by Fred Beckhusen -->
<head>
<meta charset="utf-8">
<title>Opensimulator search Objects by Outworldz.com</title>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script type="text/javascript"  src="/flexgrid/js/flexigrid.js"></script>
<link rel="stylesheet" type="text/css" href="style.css">

<script type="text/javascript">
$(document).ready(function(){
	
	$("a").click(function(){
		top.window.location.href=$(this).attr("href");
		$("#playing").html( '<p>&nbsp;Opening...</p>' );
		return true;
	})
	
	$("#flex1").flexigrid({
		url: '/Search/SearchObjects.php',
		dataType: 'json',
        method: 'GET',
        nowrap: false,
		colModel : [
			{display: 'Hop', name : 'hop', width : 35, sortable : false, align: 'left'},		
			{display: 'Name', name : 'Name', width : 200, sortable : true, align: 'left'},			
			{display: 'Description', name : 'Description', width : 160, sortable : true, align: 'left'},			
			{display: 'Region name', name : 'Regionname', width :225, sortable : true, align: 'left'},
			
			],
		
		searchitems : [
		 {display: 'Name', name :'Name', isdefault: true},
		 {display: 'Description', name :'Description', isdefault: false}
		 
		],
		sortname: 'Name',
		sortorder: 'asc',
		usepager: true,
		title: 'Click header to sort. Search button is at the bottom left.',
		useRp: true,
		rp: 100,
		showTableToggleBtn: false,
		width: 700,
		height: 400
	});


	$(document).on("click", ".hop", function(event){
		event.preventDefault();
		$("#playing").html( '<p>&nbsp;Teleporting...</p>' );
		var url = $(this).attr('href');
		window.location.href = url;
    });

});

</script>

<link rel="stylesheet" type="text/css" media="all" href="/flexgrid/css/flexigrid.css" />

<link rel="shortcut icon" href="/favicon.ico">

</head>

<body>
<div id="Links">
<a href="index.htm" target="_self"><button>Grids</button></a>
<a href="SearchRegions.htm" target="_self"><button>Regions</button></a> 
<a href="SearchParcel.htm" target="_self"><button>Parcels</button></a>
<a href="SearchObjects.htm" target="_self"><button>Objects</button></a>
<a href="SearchEvents.htm" target="_self"><button>HG Events</button></a>
<!--<a href="SearchClassifieds.htm" target="_self"><button>Classifieds</button></a>-->
&nbsp;&nbsp;&nbsp;&nbsp;
<button onclick="location.reload(true);">Refresh Page</button>
</div>
<div id="greet">
    <div id="playing"></div>
    <div id="flex1" ></div>	
</div>

</body>

</html>