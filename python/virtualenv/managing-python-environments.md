# Managing develpoment environments with Python Virutalenv

# The basic idea behind virtual environments

With `pip` we can install Python packages. By default these are installed system-wide in the same location as Python. This means that any code you write will have access to those libraries. But what if you are writing code that needs a different version of a particular library than some of your other projects? Or what if you want to experiment with different versions or different packages? Managing this all in one location could get complicated fast. But what if you could install your desired packages in separate locations and switch them out whenever you want? You can with virtual environments.

We can use the `virtualenv` Python library to manage our environments.

# General steps

First make sure you have Python and Pip installed. Then use Pip to install Virtualenv.

```bash
pip install virtualenv
```

Now you have the `virtualenv` command available in your system path.

Navigate to the root of your source code. Here you create a new folder which is where `virtualenv` will install your packages. For example:

```bash
mkdir virtualenvname
```

You should name the folder something that matches the environment because you will likely end up with more than one at some point. This is up to you but try to be precise.

Now you can tell `virtualenv` to do its thing:

```bash
virtualenv virtualenvname
```

Virtualenv installs a number of things in this folder including Python, Pip, and Wheel.

Now you can activate that environment. There are two way to do this depending on what operating system you are using. Follow the guide here.

Now you should see the name of your activated environment on your command prompt like this:

```bash
(virtualenvname)
Marnee@DESKTOP-BBKBQMF MINGW64 ~/Dropbox/github/ua-carpenties-workshops/2018-02-10-Tucson/python-lessons (marneedear/marnee-python-lessons)
```

`virtualenvname` is the name of my activated environment. Now I can pip install.

This makes Python, Pip, and Wheel installed in your virtualenv folder available on the command line and what will be used when you use those commands.

Now you can install any packages you like into your virtualenv and the files will end up in your virtualenv folder.

You can also deactivate your activated environment by doing this:

```bash
deactivate
```

It is the same for operating systems.