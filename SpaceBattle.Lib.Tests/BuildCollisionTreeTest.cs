using Hwdtech.Ioc;
using Hwdtech;
using System.Collections;

namespace SpaceBattle.Lib.Test;

public class BuildCollisionTreeTests
{
    readonly string path;
    readonly string pathwithbaddata;

    public BuildCollisionTreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var treetest = new Dictionary<int, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetCollisionTree", (object[] args) => { return treetest; }).Execute();
        path = @"../../../test_collision.txt";
        pathwithbaddata = @"../../../invalidFileData.txt";
    }

    [Fact]
    public void SuccesfulBuildCollisionTree()
    {
        var buildCollisionTree = new BuildCollisionTree(path);

        buildCollisionTree.Execute();

        var treetest = IoC.Resolve<Dictionary<int, object>>("Game.GetCollisionTree");

        Assert.True(treetest.ContainsKey(1));
        Assert.True(treetest.ContainsKey(4));
        Assert.True(treetest.ContainsKey(2));

        Assert.True(((Dictionary<int, object>)treetest[1]).ContainsKey(2));
        Assert.True(((Dictionary<int, object>)treetest[1]).ContainsKey(3));
        Assert.True(((Dictionary<int, object>)treetest[4]).ContainsKey(5));
        Assert.True(((Dictionary<int, object>)treetest[4]).ContainsKey(1));
        Assert.True(((Dictionary<int, object>)treetest[2]).ContainsKey(4));

        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)treetest[1])[2]).ContainsKey(3));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)treetest[1])[3]).ContainsKey(5));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)treetest[4])[5]).ContainsKey(6));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)treetest[4])[1]).ContainsKey(3));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)treetest[2])[4]).ContainsKey(6));

        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[1])[2])[3]).ContainsKey(4));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[1])[3])[5]).ContainsKey(7));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[4])[5])[6]).ContainsKey(7));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[4])[1])[3]).ContainsKey(9));
        Assert.True(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[2])[4])[6]).ContainsKey(8));

        Assert.False(treetest.ContainsKey(3));
        Assert.False(((Dictionary<int, object>)treetest[1]).ContainsKey(4));
        Assert.False(((Dictionary<int, object>)((Dictionary<int, object>)treetest[4])[5]).ContainsKey(8));
        Assert.False(((Dictionary<int, object>)((Dictionary<int, object>)((Dictionary<int, object>)treetest[2])[4])[6]).ContainsKey(0));
    }

    [Fact]
    public void TheFilePathIsSpecifiedIncorrectly()
    {
        var buildCollisionTree = new BuildCollisionTree("WrongSpecified");

        Assert.Throws<FileNotFoundException>(() => buildCollisionTree.Execute());
    }

    [Fact]
    public void Execute_ShouldThrowException_WhenInvalidFileData()
    {
        var buildCollisionTree = new BuildCollisionTree(pathwithbaddata);

        Assert.Throws<FormatException>(() => buildCollisionTree.Execute());
    }
}
