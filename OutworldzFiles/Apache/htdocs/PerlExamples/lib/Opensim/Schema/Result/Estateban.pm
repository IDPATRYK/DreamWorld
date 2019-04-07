use utf8;
package Opensim::Schema::Result::Estateban;

# Created by DBIx::Class::Schema::Loader
# DO NOT MODIFY THE FIRST PART OF THIS FILE

=head1 NAME

Opensim::Schema::Result::Estateban

=cut

use strict;
use warnings;

use base 'DBIx::Class::Core';

=head1 TABLE: C<estateban>

=cut

__PACKAGE__->table("estateban");

=head1 ACCESSORS

=head2 estateid

  data_type: 'integer'
  extra: {unsigned => 1}
  is_nullable: 0

=head2 banneduuid

  data_type: 'varchar'
  is_nullable: 0
  size: 36

=head2 bannedip

  data_type: 'varchar'
  is_nullable: 0
  size: 16

=head2 bannediphostmask

  data_type: 'varchar'
  is_nullable: 0
  size: 16

=head2 bannednamemask

  data_type: 'varchar'
  is_nullable: 1
  size: 64

=cut

__PACKAGE__->add_columns(
  "estateid",
  { data_type => "integer", extra => { unsigned => 1 }, is_nullable => 0 },
  "banneduuid",
  { data_type => "varchar", is_nullable => 0, size => 36 },
  "bannedip",
  { data_type => "varchar", is_nullable => 0, size => 16 },
  "bannediphostmask",
  { data_type => "varchar", is_nullable => 0, size => 16 },
  "bannednamemask",
  { data_type => "varchar", is_nullable => 1, size => 64 },
);


# Created by DBIx::Class::Schema::Loader v0.07042 @ 2014-10-14 12:09:34
# DO NOT MODIFY THIS OR ANYTHING ABOVE! md5sum:xMkQjAZ4ABhow6lODrpGeA


# You can replace this text with custom code or comments, and it will be preserved on regeneration
1;
