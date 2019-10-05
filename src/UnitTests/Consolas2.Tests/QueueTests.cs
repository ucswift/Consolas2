using System;
using FluentAssertions;
using NUnit.Framework;

namespace Consolas2.Core.Tests
{
    [TestFixture]
    public class QueueTests
    {
        [Test]
        public void ReferenceQueueStory()
        {
            var que = new System.Collections.Generic.Queue<string>();

            que.Enqueue("foo");
            que.Count.Should().Be(1);
            que.Peek().Should().Be("foo");
            que.Dequeue().Should().Be("foo");
            que.Count.Should().Be(0);

            que.Enqueue("foo");
            que.Enqueue("bar");
            que.Count.Should().Be(2);
            que.Peek().Should().Be("foo");
            que.Dequeue().Should().Be("foo");
            que.Dequeue().Should().Be("bar");
            que.Count.Should().Be(0);

            que.Enqueue("foo");
            que.Enqueue("bar");
            que.Enqueue("baz");
            que.Clear();
            que.Should().BeEmpty();
        }

        [Test]
        public void QueueStory()
        {
            var que = new Queue<string>();

            que.Enqueue("foo");
            que.Count.Should().Be(1);
            que.Peek().Should().Be("foo");
            que.Dequeue().Should().Be("foo");
            que.Count.Should().Be(0);

            que.Enqueue("foo");
            que.Enqueue("bar");
            que.Count.Should().Be(2);
            que.Peek().Should().Be("foo");
            que.Dequeue().Should().Be("foo");
            que.Dequeue().Should().Be("bar");
            que.Count.Should().Be(0);

            que.Enqueue("foo");
            que.Enqueue("bar");
            que.Enqueue("baz");
            que.Clear();
            que.Should().BeEmpty();
        }

        [Test]
        public void DeDequeue()
        {
            var que = new Queue<string>();

            que.Enqueue("foo");
            que.Enqueue("bar");

            que.Dequeue();
            que.Enqueue("baz");
            que.Peek().Should().Be("bar");

            que.DeDequeue("foo");
            que.Peek().Should().Be("foo");

            que.Clear();
            que.DeDequeue("bar");
            que.Peek().Should().Be("bar");
        }

        [Test]
        public void Peek_Empty_ThrowsException()
        {
            var que = new Queue<string>();
            Assert.Throws<InvalidOperationException>(() => que.Peek());
        }

        [Test]
        public void Dequeue_Empty_ThrowsException()
        {
            var que = new Queue<string>();
            Assert.Throws<InvalidOperationException>(() => que.Dequeue());
        }

        [Test]
        public void CopyTo_Array_CopiesItemsToArray()
        {
            var que = new Queue<string>();
            que.Enqueue("foo");

            var array = new string[1];
            que.CopyTo(array, 0);

            array.Should().Equal(new []{"foo"});
        }

        [Test]
        public void SyncRoot_ShouldNotBeNull()
        {
            var que = new Queue<int>();
            que.SyncRoot.Should().NotBeNull();
        }

        [Test]
        public void IsSynchronized_ShouldBeFalse()
        {
            var que = new Queue<int>();
            que.IsSynchronized.Should().BeFalse();
        }
    }
}
