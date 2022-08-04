using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Tsgcpp.Localization.Extension.Editor;

namespace Tests.Tsgcpp.Localization.Extension.Editor
{
    public class TestCacheListConverter
    {
        CacheListConverter<string, object> _target;
        ListConverterMock _listConverterMock;

        [SetUp]
        public void SetUp()
        {
            _listConverterMock = new ListConverterMock();
            _target = new CacheListConverter<string, object>(_listConverterMock);
        }

        [Test]
        public void Convert_ReturnsNewResultList_IfFirstCall()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };

            // when
            var actual = _target.Convert(list);

            // then
            Assert.That(actual, Is.SameAs(_listConverterMock.ResultList));
            Assert.That(_listConverterMock.LastArg, Is.SameAs(list));
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(1));
        }

        [Test]
        public void Convert_ReturnsNewResultList_IfSecondCallWithAnother()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };
            var list2 = new List<string> { "a", "c" };
            _target.Convert(list);

            var resultList2 = new List<object> { 2, "b" };
            _listConverterMock.ResultList = resultList2;

            // when
            var actual = _target.Convert(list2);

            // then
            Assert.That(actual, Is.SameAs(resultList2));
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(2));
        }

        [Test]
        public void Convert_ReturnsCachedResultList_IfSecondCallWithTheSameList()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };
            _target.Convert(list);

            // when
            var actual = _target.Convert(list);

            // then
            Assert.That(actual, Is.SameAs(_listConverterMock.ResultList));
            // Not call ListConverterMock.Convert() because the cache is already created.
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(1));
        }

        [Test]
        public void Convert_ReturnsCachedResultList_IfSecondCallWithTheSameValues()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };
            var list2 = new List<string> { "a", "b", "c" };
            _target.Convert(list);

            // when
            var actual = _target.Convert(list2);

            // then
            Assert.That(actual, Is.SameAs(_listConverterMock.ResultList));
            // Not call ListConverterMock.Convert() because the cache is already created.
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(1));
        }

        [Test]
        public void Convert_ReturnsNewResultList_IfSecondCallWithAnotherValuesOfSameCount()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };
            var list2 = new List<string> { "a", "c", "d" };
            _target.Convert(list);

            var resultList2 = new List<object> { 2, "b" };
            _listConverterMock.ResultList = resultList2;

            // when
            var actual = _target.Convert(list2);

            // then
            Assert.That(actual, Is.SameAs(resultList2));
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(2));
        }

        [Test]
        public void Convert_ReturnsNewResultList_IfSecondCallWithAnotherValuesOfDifferentCount()
        {
            // setup
            var list = new List<string> { "a", "b", "c" };
            var list2 = new List<string> { "a", "b" };
            _target.Convert(list);

            var resultList2 = new List<object> { 2, "b" };
            _listConverterMock.ResultList = resultList2;

            // when
            var actual = _target.Convert(list2);

            // then
            Assert.That(actual, Is.SameAs(resultList2));
            Assert.That(_listConverterMock.ConvertCount, Is.EqualTo(2));
        }

        [Test]
        public void Convert_ThrowsNullReferenceException_IfListIsNull()
        {
            Assert.Throws<NullReferenceException>(() => _target.Convert(null));
        }

        public class ListConverterMock : IListConverter<string, object>
        {
            public IReadOnlyList<object> ResultList { get; set; } = new List<object> { 1, "a", null };
            public IReadOnlyList<string> LastArg { get; set; } = null;
            public int ConvertCount { get; set; } = 0;

            public IReadOnlyList<object> Convert(IReadOnlyList<string> list)
            {
                LastArg = list;
                ConvertCount += 1;
                return ResultList;
            }
        }
    }
}
