__author__ = 'Marnee Dearman'
from py2neo import Graph, Node, Relationship
from settings import graphene

# What is the URL to my NEO4J
tuple_graph = Graph(graphene.DATABASE_URL)
print tuple_graph

# start over with delete so I can run the whole script at one time
tuple_graph.delete_all()

# Let's try modeling the group
# using Py2Neo.  This is the Python Meetup after all

# Example CYPHER
# CREATE (m:MEMBER {name:"Marnee"} )
# RETURN m
# Create a member aliased as "m" with name "Marnee"
# Return that member's node (m)

# Example PY2NEO
# one way is to setup a dictionary with the properties for the new Node, in this case MEMBER
# member_properties = {}
# member_properties["name"] = "Julian"
# show the code for py2neo
# member_node = Node.cast("MEMBER", member_properties)
# member_node = Node.cast("MEMBER", name="Julian", python_years=5)
# tuple_graph.create(member_node)

# Julian is lonely, let's give him a friend

# member_node = Node.cast("MEMBER", name="Marnee", python_years=1)
# tuple_graph.create(member_node)

# but they don't have a relationship
# let's have them meet
# create a relationship between two nodes
# Example CYPHER create nodes and relationships at the same time
# CREATE (marnee:MEMBER {name:"Marnee"}),(julian:MEMBER {name:"Julian"}),
#   (marnee)-[k:KNOWS {since:2005}]->(julian)
# RETURN marnee, k, julian
# but if the nodes exist then we end up with duplicates
# so you can MATCH first to find the nodes that exist and then create relationship between them
# MATCH (a:MEMBER { name: 'Marnee' }), (b:MEMBER { name: 'Julian' })
# CREATE (a)-[:KNOWS]->(b)
# OR MERGE which will create the node and then make the relationship
# MERGE (a:MEMBER {name:'Marnee'})
# MERGE (b:MEMBER {name: 'Julian'})
# create (a)-[:KNOWS]->(b)
# but it will now duplicate the realtionship so we can use CREATE UNIQUE
# MERGE (a:MEMBER {name:'Marnee'})
# MERGE (b:MEMBER {name: 'Julian'})
# create unique (a)-[:KNOWS]->(b)

# MATCH (a:MEMBER { name: 'Marnee' }), (b:MEMBER { name: 'Julian' })
# CREATE UNIQUE (a)-[:KNOWS]->(b)
#
# py2neo helps us prevent duplicate nodes when we create them to
# create_unique

# Keep building