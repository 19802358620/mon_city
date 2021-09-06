var yn = "y";
function yn_check(yn,url)
{
	if (yn=="y")
	{
		window.location=url;
  	}
  	else
  	{
		window.open( "notice.htm","WinShow","width=400,height=280,resizable=no,scrollbars=no,menubar=no,status=0");
	}
}
function MM_swapImgRestore()
{
	var i,x,a=document.MM_sr; 
	for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++)
	{
		x.src=x.oSrc;
	}
}
function MM_findObj(n, d)
{
	var p,i,x;
	if(!d)
	{
		d=document;
		if((p=n.indexOf("?"))>0&&parent.frames.length)
		{
    			d=parent.frames[n.substring(p+1)].document;
    			n=n.substring(0,p);
    		}
  		if(!(x=d[n])&&d.all)
  		{
  			x=d.all[n];
  		}
  		for (i=0;!x&&i<d.forms.length;i++)
  		{
  			x=d.forms[i][n];
  		}
  		for(i=0;!x&&d.layers&&i<d.layers.length;i++)
  		{
  			x=MM_findObj(n,d.layers[i].document);
  		}
  		if(!x && d.getElementById)
  		{
  			x=d.getElementById(n);
  		}
  		return x;
  	}
}

function MM_swapImage()
{
	var i,j=0,x,a=MM_swapImage.arguments;
	document.MM_sr=new Array;
	for(i=0;i<(a.length-2);i+=3)
	{
   		if ((x=MM_findObj(a[i]))!=null)
   		{
   			document.MM_sr[j++]=x;
   		}
   		if(!x.oSrc)
   		{
   			x.oSrc=x.src;
   		} 
   		x.src=a[i+2];
   	}
}

function getWord(sw)//获取鼠标选中的文本
{
	var str = document.selection.createRange().text;
	if(typeof(str)!="undefined")
	{
		if(str!="")
	        {
				if(typeof(document.all.searchw)!="undefined")
			{
				document.all.searchw.value=str;
			}
		}
	}
}
