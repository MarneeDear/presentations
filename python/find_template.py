__author__ = 'Marnee Dearman'
from py2neo import Graph, Node, Relationship
from settings import graphene

graph = Graph(graphene.DATABASE_URL)
print graph

# find a node or set of nodes according to properties and labels
# graph.find_one() # returns a single node
# graph.find() # returns a generator

# Let's find Marnee
marnee_node = graph.find_one("Person", property_key="name", property_value="Marnee")
print "find_one Marnee %s" % marnee_node

marnee_generator = graph.find("Person", property_key="name", property_value="Marnee")
for marnee in marnee_generator:
    print marnee

# Let's find Julian
julian_node = graph.find_one("Person", property_key="name", property_value="Julian")
print "find_one Julian %s" % julian_node

# Let's find all the Persons Julian knows
# show the Cypher -- MATCH
# show the code
# graph.match()
# graph.match_one()



julian_knows = graph.match(start_node=julian_node,
                           rel_type="KNOWS",
                           end_node=None)
for friend in julian_knows:
    print "friend %s" % friend

# looks like Julian has no friends.  that is because the relationship goes the other way
# show graph

#who knows Julian
knows_julian = graph.match(start_node=None,
                           rel_type="KNOWS",
                           end_node=julian_node)
for friend_relationship in knows_julian:
    print "friend %s" % friend_relationship  # this looks like Cypher -- how do I use this?

knows_julian = graph.match(start_node=None,
                           rel_type="KNOWS",
                           end_node=julian_node)

for friend_relationship in knows_julian:
    print friend_relationship.start_node.properties

knows_julian = graph.match(start_node=julian_node,
                           rel_type="KNOWS",
                           end_node=None, bidirectional=True)

for friend_relationship in knows_julian:
    print friend_relationship.start_node.properties

# You can also do things to Nodes like
# match all relationships into this node
matches = julian_node.match_incoming()
for match in matches:
    print match

# match all relationships out of this node
matches = julian_node.match_outgoing()
for match in matches:
    print match

