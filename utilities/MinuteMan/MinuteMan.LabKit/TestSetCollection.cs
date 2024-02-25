using System.Reflection;

namespace MinuteMan.LabKit;

public class TestSetCollection {
    private Dictionary<Type, List<MethodInfo>> TestSets { get; init; }

    public TestSetCollection(Dictionary<Type, List<MethodInfo>> testSets) {
        //  copy the test set dictionary
        TestSets = testSets
            .ToDictionary(e => e.Key, e => e.Value);
    }

    public IEnumerator<TestSet> GetTestSetIterator() {
        foreach(var set in TestSets) {
            yield return new DefaultTestSet(set);
        }
    }

    private class DefaultTestSet : TestSet {

        internal DefaultTestSet(KeyValuePair<Type, List<MethodInfo>> testSetInfo) : base(testSetInfo) {

        }
    }
}
