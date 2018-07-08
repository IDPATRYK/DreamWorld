
my $type  = '-V2.2';# '-Beta-V1.5';
my $dir = "E:\\Opensim\\Outworldz Dreamgrid Source";

chdir ($dir);


use File::Copy;
use File::Path;
use 5.010;

my @deletions = (
				 "$dir/OutworldzFiles/AutoBackup",
				 "$dir/OutworldzFiles/Opensim/bin/assetcache",
				 "$dir/OutworldzFiles/Opensim/bin/j2kDecodeCache",
				 "$dir/OutworldzFiles/Opensim/bin/MeshCache",
				 "$dir/OutworldzFiles/Opensim/bin/autobackup",
				 "$dir/OutworldzFiles/Opensim/bin/ScriptEngines",
				 "$dir/OutworldzFiles/Opensim/bin/maptiles",
				 "$dir/OutworldzFiles/Opensim/bin/Regions",
				 "$dir/OutworldzFiles/Opensim/bin/bakes",
				 "$dir/OutworldzFiles/mysql/data/opensim",
				 "$dir/OutworldzFiles/mysql/data/robust",
				 "$dir/OutworldzFiles/mysql/data/addin-db-002",
				 "$dir/OutworldzFiles/mysql/data/fsassets",
				 
				 
				 );

foreach my $path ( @deletions) {
	rm($path);
}

#clean up opensim
unlink "$dir/OutworldzFiles/Opensim/bin/Opensim.log" ;
unlink "$dir/OutworldzFiles/Opensim/bin/Opensimstats.log" ;
unlink "$dir/Icecast/error.log" ;
unlink "$dir/Icecast/access.log" ;

unlink "$dir/OutworldzFiles/Opensim/bin/OpensimConsoleHistory.txt" ;
unlink "$dir/OutworldzFiles/Opensim-0.9/bin/OpensimConsoleHistory.txt" ;
unlink "$dir/OutworldzFiles/Opensim/bin/LocalUserStatistics.db" ;

#Setting
unlink "$dir/Outworldzfiles/Settings.ini" ;


#logs
unlink "$dir/OutworldzFiles/Diagnostics.log" ;
unlink "$dir/OutworldzFiles/Outworldz.log" ;
unlink "$dir/OutworldzFiles/Init.txt" ;
unlink "$dir/OutworldzFiles/upnp.log" ;
unlink "$dir/OutworldzFiles/http.log" ;

unlink "../Zips/DreamGrid$type.zip" ;
unlink "../Zips/Outworldz-Update$type.zip" ;

say ("Start Mysql and wait for it to come up:");
<STDIN>;

chdir(qq!$dir/OutworldzFiles/mysql/bin/!);

print `mysqlcheck.exe --port 3306 -u root -r mysql`;
print `mysqlcheck.exe --port 3306 -u root -r opensim`;
print `mysqlcheck.exe --port 3306 -u root -r robust`;
print `mysqladmin.exe --port 3306 -u root shutdown`;


#mysql
unlink "$dir/OutworldzFiles/mysql/data/Alienware.err" ;
unlink "$dir/OutworldzFiles/mysql/data/Alienware.pid" ;
unlink	"$dir/OutworldzFiles/mysql/data/ib_logfile0" || die;
unlink	"$dir/OutworldzFiles/mysql/data/ib_logfile1" || die;
unlink	"$dir/OutworldzFiles/mysql/data/ibdata1" || die;


chdir ($dir);
# SIGN FIRST



print "Process Main Zip?\n";
<STDIN>;

my @files =   `cmd /c dir /b `;

foreach my $file (@files) {
	chomp $file;
	next if -d $file;
	#next if $file eq 'Make_zip_v2.pl';
	next if $file =~ /^\./;
	print  "$file ";
	Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid$type.zip \"$dir\\$file\" ");
}

say("Adding folders");
#Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid-Update$type.zip Licenses_to_Content");
#Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid-Update$type.zip OutworldzFiles");
#Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid-Update$type.zip Viewer");

Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid$type.zip Licenses_to_Content");
Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid$type.zip OutworldzFiles");
Process ("../7z.exe -tzip a ..\\Zips\\DreamGrid$type.zip Icecast");

		
say("Updater Build");
if (!copy ("../Zips/DreamGrid$type.zip", "../Zips/DreamGrid-Update$type.zip"))  {die $!;}

say("Drop mysql files from update");
# now delete the mysql from the UPDATE
Process ("../7z.exe -tzip d ..\\Zips\\DreamGrid-Update$type.zip Outworldzfiles\\mysql\\data\\ -r ");

say ("Dropping Setting.ini from updater");
Process ("../7z.exe -tzip d ..\\Zips\\DreamGrid-Update$type.zip Outworldzfiles\\Settings.ini -r ");

# del Dot net because we cannot overwrite an open file
Process ("../7z.exe -tzip d ..\\Zips\\DreamGrid-Update$type.zip DotNetZip.dll ");

#####################
print "Server Copy\n";

# Ready to move it all
unlink "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid$type.zip";
if (!copy ("../Zips/DreamGrid$type.zip", "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid$type.zip"))  {die $!;}
unlink "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip";
if (!copy ("../Zips/DreamGrid$type.zip", "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid.zip"))  {die $!;}

#web server
print "Server Copy Update\n";
unlink "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update$type.zip";
if (!copy ("../Zips/DreamGrid-Update$type.zip", "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update$type.zip"))  {die $!;}
unlink "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip";
if (!copy ("../Zips/DreamGrid-Update$type.zip", "y:/Inetpub/Secondlife/Outworldz_Installer/Grid/DreamGrid-Update.zip"))  {die $!;}

# lastly revisions file
if (!copy ('Revisions.txt', 'y:/Inetpub/Secondlife/Outworldz_Installer/Revisions.txt'))  {die $!;}





print "Done!";

sub Write
{
	my $dest = shift;
	my $content = shift;
	open OUT, ">$dest" || die $!;
	print OUT $content;
	close OUT;
}

sub Process
{
	my $file = shift;
	
	my $x = `$file`;
	if ($x =~ /Everything is Ok/) {
		print "Ok\n";
	} else {
		print "Fail: $x\n";
		exit;
	}
}

sub rm {
	
my $path = shift;
	
	my $errors;
	while ($_ = glob("'$path/*'")) {
		rmtree($_)
		  or ++$errors, warn("Can't remove $_: $!");
	}
	
	#exit(1) if $errors;
}
