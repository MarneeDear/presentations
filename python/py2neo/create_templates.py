__author__ = 'Marnee Dearman'
from py2neo import Graph, Node, Relationship
from settings import graphene

# make a graph object pointed to my graphene database (hopefully I am online)
graph = Graph(graphene.DATABASE_URL)
print graph

# CREATE A NODE
# there are a couple of ways to do this
# in my own projects I was passing around json and object properties in dictionaries so I just used that
# to create a py2neo Node and then pass that to the py2neo create function
# but you can, of course, use args and kwargs
# let's do it both ways

# CREATE (n:Person { name: 'Marnee' }) RETURN n
marnee_node = Node.cast("Person", name="Marnee", hello="World", age=100) # not just strings
print marnee_node
graph.create(marnee_node)

marnee_properties = {}
marnee_properties["name"] = "Marnee"
marnee_properties["hello"] = "World"
marnee_properties["age"] = 100
marnee_with_dict_node = Node.cast("Person", marnee_properties)
print marnee_with_dict_node
graph.create(marnee_with_dict_node)
# look at the graph
# BUT I END UP WITH GHOLA, I mean DUPLICATES
# How can we do this without duplication????

# Show merge in the browser

# PY2NEO
# graph.merge() -- returns a generator (generators are cool)
# graph.merge_one() -- returns one node
# show documentation

marnee_merge_node = graph.merge_one(label="Person", property_key="name", property_value="Marnee")
print marnee_merge_node

# but I have more than one property on this node.  How do I get them in there
# merge returns a node, or set of nodes, and we can do things to a Node like Node.Push
# Node.properties
# Node.push()

for key, value in marnee_properties.iteritems():  # so pythonic
            marnee_merge_node[key] = value
marnee_merge_node.push()
#look at the graph.  did we create a third Marnee?  No we only have two.

#How many Marnees do you know?
# comment out marnee dict and clear db to start over

# let's try creating a relationship
# First the gholas need a friend -- I volunteer Julian
julian_properties = {}
julian_properties["hello"] = "World"
julian_properties["age"] = 1000  # damn you are old!

julian_node = graph.merge_one(label="Person", property_key="name", property_value="Julian")
print julian_node
for key, value in julian_properties.iteritems():  # so pythonic
            julian_node[key] = value
julian_node.push()

# Now we have to Persons in the graph.  How do we make a relationship?
# we have out two Node objects above
# now we need a Relationship

knows = Relationship(marnee_merge_node, "KNOWS", julian_node)
graph.create_unique(knows)

# look at the graph
# does Marnee know Julian?  Yes but only the one

